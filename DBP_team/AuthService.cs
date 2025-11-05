using System;
using System.Data;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public class AuthService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 10000;

        /// <summary>
        /// 회원가입: companyId/departmentId/teamId는 0이면 NULL로 저장됩니다.
        /// 반환값: 성공 여부. 실패 시 error에 메시지 반환.
        /// </summary>
        public bool Register(string email, string password, string fullName, int companyId, int departmentId, int teamId, out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(fullName))
            {
                error = "이메일, 비밀번호, 이름은 필수입니다.";
                return false;
            }

            try
            {
                // 이메일 중복 확인
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
                            VALUES (@companyId, @departmentId, @teamId, @email, @hash, @fullName)";

                var pCompany = new MySqlParameter("@companyId", companyId > 0 ? (object)companyId : DBNull.Value);
                var pDept = new MySqlParameter("@departmentId", departmentId > 0 ? (object)departmentId : DBNull.Value);
                var pTeam = new MySqlParameter("@teamId", teamId > 0 ? (object)teamId : DBNull.Value);

                var affected = DBManager.Instance.ExecuteNonQuery(sql,
                    pCompany,
                    pDept,
                    pTeam,
                    new MySqlParameter("@email", email),
                    new MySqlParameter("@hash", hash),
                    new MySqlParameter("@fullName", fullName));

                if (affected <= 0)
                {
                    error = "회원가입 중 DB 오류가 발생했습니다.";
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = "회원가입 오류: " + ex.Message;
                return false;
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

        public bool VerifyPassword(string password, string stored)
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
                    var diff = 0;
                    for (int i = 0; i < storedHash.Length; i++) diff |= storedHash[i] ^ computed[i];
                    return diff == 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
