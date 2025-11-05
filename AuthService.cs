DBP_team\AuthService.cs
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using DBP_team.Models;

namespace DBP_team
{
    /// <summary>
    /// 인증 관련 기능: 회원가입(Register) 및 로그인(Authenticate)
    /// 비밀번호는 PBKDF2 (Rfc2898DeriveBytes)로 해시화하여 저장합니다.
    /// 저장 포맷: iterations:saltBase64:hashBase64
    /// </summary>
    public class AuthService
    {
        private const int SaltSize = 16; // 바이트
        private const int HashSize = 32; // 바이트
        private const int Iterations = 10000;

        public bool Register(string email, string password, string fullName, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                error = "이메일, 비밀번호, 이름을 모두 입력하세요.";
                return false;
            }

            try
            {
                // 이메일 중복 체크
                var existsObj = DBManager.Instance.ExecuteScalar(
                    "SELECT COUNT(*) FROM users WHERE email = @email",
                    new MySqlParameter("@email", email));
                var exists = Convert.ToInt32(existsObj);

                if (exists > 0)
                {
                    error = "이미 등록된 이메일입니다.";
                    return false;
                }

                var hash = HashPassword(password);

                var sql = @"INSERT INTO users (company_id, department_id, team_id, email, password_hash, full_name)
                            VALUES (NULL, NULL, NULL, @email, @hash, @fullName)";

                var affected = DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@hash", hash),
                    new MySqlParameter("@fullName", fullName));

                if (affected <= 0)
                {
                    error = "회원가입 실패(데이터베이스 오류).";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = "회원가입 중 오류: " + ex.Message;
                return false;
            }
        }

        public User Authenticate(string email, string password, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                error = "이메일과 비밀번호를 입력하세요.";
                return null;
            }

            try
            {
                var dt = DBManager.Instance.ExecuteDataTable(
                    "SELECT id, company_id, department_id, team_id, password_hash, full_name, role, created_at, last_login FROM users WHERE email = @email",
                    new MySqlParameter("@email", email));

                if (dt.Rows.Count == 0)
                {
                    error = "존재하지 않는 계정입니다.";
                    return null;
                }

                var row = dt.Rows[0];
                var storedHash = row["password_hash"]?.ToString();

                if (!VerifyPassword(password, storedHash))
                {
                    error = "비밀번호가 일치하지 않습니다.";
                    return null;
                }

                var user = new User
                {
                    Id = Convert.ToInt32(row["id"]),
                    CompanyId = row["company_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["company_id"]),
                    DepartmentId = row["department_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["department_id"]),
                    TeamId = row["team_id"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["team_id"]),
                    Email = email,
                    FullName = row["full_name"]?.ToString(),
                    Role = row["role"]?.ToString(),
                    CreatedAt = row["created_at"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["created_at"]),
                    LastLogin = row["last_login"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["last_login"])
                };

                // 마지막 로그인 시간 업데이트
                DBManager.Instance.ExecuteNonQuery(
                    "UPDATE users SET last_login = NOW() WHERE id = @id",
                    new MySqlParameter("@id", user.Id));

                return user;
            }
            catch (Exception ex)
            {
                error = "로그인 중 오류: " + ex.Message;
                return null;
            }
        }

        private string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    var hash = pbkdf2.GetBytes(HashSize);
                    return $"{Iterations}:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
                }
            }
        }

        private bool VerifyPassword(string password, string stored)
        {
            if (string.IsNullOrEmpty(stored)) return false;

            try
            {
                var parts = stored.Split(':');
                if (parts.Length != 3) return false;

                var iterations = int.Parse(parts[0]);
                var salt = Convert.FromBase64String(parts[1]);
                var storedHash = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    var computed = pbkdf2.GetBytes(storedHash.Length);
                    // 고정 시간 비교
                    return AreEqual(storedHash, computed);
                }
            }
            catch
            {
                return false;
            }
        }

        private bool AreEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;
            var diff = 0;
            for (int i = 0; i < a.Length; i++)
                diff |= a[i] ^ b[i];
            return diff == 0;
        }
    }
}