using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    /// <summary>
    /// 스레드 안전한 DB 접근 유틸리티. 각 호출마다 새 연결을 생성/폐기하여
    /// 동시에 DataReader/Adapter를 사용하는 시나리오에서도 충돌이 발생하지 않도록 합니다.
    /// </summary>
    public sealed class DBManager : IDisposable
    {
        private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(() => new DBManager());
        public static DBManager Instance => _instance.Value;

        private readonly string _connectionString;
        private bool _disposed;

        private DBManager()
        {
            _connectionString = GetConnectionString();
        }

        private string GetConnectionString()
        {
            return "Server=223.130.151.111;Port=3306;Database=s5701576;Uid=s5701576;Pwd=s5701576;";
        }

        private MySqlConnection CreateOpenConnection()
        {
            var conn = new MySqlConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                using (var adapter = new MySqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (var conn = CreateOpenConnection())
            using (var cmd = new MySqlCommand(sql, conn))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }

        public MySqlDataReader ExecuteReader(string sql, params MySqlParameter[] parameters)
        {
            var conn = CreateOpenConnection();
            var cmd = new MySqlCommand(sql, conn);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            // 연결은 리더가 닫힐 때 자동으로 닫히게 합니다.
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        public bool TestConnection(out string error)
        {
            error = null;
            try
            {
                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("SELECT 1", conn))
                    {
                        var r = cmd.ExecuteScalar();
                        if (r == null)
                        {
                            error = "서버에서 응답이 없습니다 (NULL).";
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message + (ex.InnerException != null ? " | " + ex.InnerException.Message : "");
                return false;
            }
        }

        public static void LogUserActivity(int userId, string activityType)
        {
            if (userId <= 0 || string.IsNullOrWhiteSpace(activityType)) return;

            const string sql = "INSERT INTO user_activity_logs (user_id, activity_type) VALUES (@userId, @activityType)";
            try
            {
                Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@userId", userId),
                    new MySqlParameter("@activityType", activityType));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"사용자 활동 로그 기록 실패: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
