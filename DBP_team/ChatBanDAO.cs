using System;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public static class ChatBanDAO
    {
        // Returns true if the pair is banned, otherwise false
        public static bool IsChatBanned(int user1, int user2)
        {
            if (user1 <= 0 || user2 <= 0) return false;
            if (user1 == user2) return false; // self chat allowed

            int a = Math.Min(user1, user2);
            int b = Math.Max(user1, user2);

            try
            {
                object obj = DBManager.Instance.ExecuteScalar(
                    "SELECT COUNT(*) FROM chat_bans WHERE user_id_1 = @u1 AND user_id_2 = @u2",
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
    }
}
