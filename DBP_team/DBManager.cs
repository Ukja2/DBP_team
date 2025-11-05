using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    /// <summary>
    /// 애플리케이션 전체에서 하나의 MySQL 연결을 공유하는 스레드 안전한 싱글턴입니다.
    /// MySqlConnection 생성은 지연(Lazy)하며, 연결 문자열은 하드코드(개발용) 상태입니다.
    /// </summary>
    public sealed class DBManager : IDisposable
    {
        private static readonly Lazy<DBManager> _instance = new Lazy<DBManager>(() => new DBManager());
        public static DBManager Instance => _instance.Value;

        private readonly string _connectionString;
        private MySqlConnection _connection;
        private bool _disposed;

        private DBManager()
        {
            _connectionString = GetConnectionString();
            // MySqlConnection 생성/파싱은 실제 사용 시점으로 지연합니다.
        }

        private string GetConnectionString()
        {
            // 하드코드된 개발용 연결 문자열
            // 주: SslMode 값 때문에 문제가 발생했으므로 기본적으로 SslMode를 제거합니다.
            // 필요하면 사용 중인 MySql.Data 버전에 맞는 값으로 설정하세요.
            return "Server=223.130.151.111;Port=3306;Database=s5701576;Uid=s5701576;Pwd=s5701576;";
        }

        /// <summary>
        /// 현재 열린 연결을 반환합니다. 연결 객체를 지연 생성하며 닫혀있으면 Open합니다.
        /// </summary>
        public MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    // MySqlConnection 생성 시 connection string 파싱이 일어남
                    _connection = new MySqlConnection(_connectionString);
                }

                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }

                return _connection;
            }
        }

        public DataTable ExecuteDataTable(string sql, params MySqlParameter[] parameters)
        {
            using (var cmd = new MySqlCommand(sql, Connection))
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
            using (var cmd = new MySqlCommand(sql, Connection))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteNonQuery();
            }
        }

        public object ExecuteScalar(string sql, params MySqlParameter[] parameters)
        {
            using (var cmd = new MySqlCommand(sql, Connection))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                return cmd.ExecuteScalar();
            }
        }

        public MySqlDataReader ExecuteReader(string sql, params MySqlParameter[] parameters)
        {
            var cmd = new MySqlCommand(sql, Connection);
            if (parameters != null && parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// DB 연결 테스트(안전하게 새 연결을 만들어 테스트함).
        /// </summary>
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

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                if (_connection != null)
                {
                    try
                    {
                        if (_connection.State != ConnectionState.Closed)
                            _connection.Close();
                    }
                    catch { /* 무시 */ }

                    _connection.Dispose();
                    _connection = null;
                }
            }
            finally
            {
                _disposed = true;
            }
        }
    }
}
