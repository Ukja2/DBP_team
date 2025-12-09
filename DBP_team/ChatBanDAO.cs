using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public static class ChatBanDAO
    {
        private static void EnsureChatBansTableExists()
        {
            try
            {
                // Simplified schema without generated columns to ensure compatibility across MySQL versions
                DBManager.Instance.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS chat_bans (" +
                    " id INT NOT NULL AUTO_INCREMENT, " +
                    " user_id_1 INT NOT NULL, " +
                    " user_id_2 INT NOT NULL, " +
                    " created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP, " +
                    " PRIMARY KEY (id), " +
                    " UNIQUE KEY uq_chat_bans_pair (user_id_1, user_id_2), " +
                    " KEY ix_bans_user1 (user_id_1), " +
                    " KEY ix_bans_user2 (user_id_2) " +
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;");
            }
            catch { }
        }

        // Returns true if the pair is banned, otherwise false
        public static bool IsChatBanned(int user1, int user2)
        {
            if (user1 <= 0 || user2 <= 0) return false;
            if (user1 == user2) return false; // self chat allowed

            int a = Math.Min(user1, user2);
            int b = Math.Max(user1, user2);

            try
            {
                EnsureChatBansTableExists();
                object obj = DBManager.Instance.ExecuteScalar(
                    "SELECT COUNT(*) FROM chat_bans WHERE (user_id_1 = @u1 AND user_id_2 = @u2) OR (user_id_1 = @u2 AND user_id_2 = @u1)",
                    new MySqlParameter("@u1", a),
                    new MySqlParameter("@u2", b));
                int count = 0;
                if (obj != null && obj != DBNull.Value)
                {
                    int.TryParse(obj.ToString(), out count);
                }
                return count > 0;
            }
            catch
            {
                // On error, do not block chat by default
                return false;
            }
        }

        public static bool AddBan(int user1, int user2)
        {
            if (user1 <= 0 || user2 <= 0) return false;
            if (user1 == user2) return false;
            int a = Math.Min(user1, user2);
            int b = Math.Max(user1, user2);
            try
            {
                EnsureChatBansTableExists();
                // Normalize pair order and avoid duplicates
                var sql = "INSERT IGNORE INTO chat_bans (user_id_1, user_id_2) VALUES (@a, @b)";
                var rows = DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@a", a),
                    new MySqlParameter("@b", b));
                return rows > 0;
            }
            catch { return false; }
        }

        public static bool RemoveBan(int user1, int user2)
        {
            if (user1 <= 0 || user2 <= 0) return false;
            int a = Math.Min(user1, user2);
            int b = Math.Max(user1, user2);
            try
            {
                EnsureChatBansTableExists();
                var sql = "DELETE FROM chat_bans WHERE (user_id_1 = @a AND user_id_2 = @b) OR (user_id_1 = @b AND user_id_2 = @a)";
                var rows = DBManager.Instance.ExecuteNonQuery(sql,
                    new MySqlParameter("@a", a),
                    new MySqlParameter("@b", b));
                return rows > 0;
            }
            catch { return false; }
        }

        public static DataTable LoadBans()
        {
            try
            {
                EnsureChatBansTableExists();
                var sql = "SELECT cb.user_id_1, cb.user_id_2, cb.created_at, " +
                          " COALESCE(u1.full_name,u1.email) AS name1, " +
                          " COALESCE(u2.full_name,u2.email) AS name2 " +
                          "FROM chat_bans cb " +
                          "LEFT JOIN users u1 ON u1.id = cb.user_id_1 " +
                          "LEFT JOIN users u2 ON u2.id = cb.user_id_2 " +
                          "ORDER BY cb.created_at DESC";
                return DBManager.Instance.ExecuteDataTable(sql);
            }
            catch { return new DataTable(); }
        }
    }
}
