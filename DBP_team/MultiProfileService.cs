using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace DBP_team
{
    public static class MultiProfileService
    {
        public static void EnsureTables()
        {
            try
            {
                DBManager.Instance.ExecuteNonQuery(
                    "CREATE TABLE IF NOT EXISTS multi_profile_map ("+
                    "  id INT AUTO_INCREMENT PRIMARY KEY,\n"+
                    "  owner_user_id INT NOT NULL,\n"+
                    "  target_user_id INT NOT NULL,\n"+
                    "  display_name VARCHAR(255) NULL,\n"+
                    "  photo LONGBLOB NULL,\n"+
                    "  created_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,\n"+
                    "  updated_at DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,\n"+
                    "  UNIQUE KEY uk_owner_target (owner_user_id, target_user_id),\n"+
                    "  INDEX idx_owner (owner_user_id),\n"+
                    "  INDEX idx_target (target_user_id)\n"+
                    ") ENGINE=InnoDB DEFAULT CHARSET=utf8mb4");
            }
            catch { }
        }

        public static DataTable GetMappings(int ownerUserId)
        {
            EnsureTables();
            var sql = "SELECT mpm.id, mpm.display_name, mpm.target_user_id, COALESCE(u.full_name,u.email) AS target_name, mpm.created_at, mpm.updated_at FROM multi_profile_map mpm LEFT JOIN users u ON u.id = mpm.target_user_id WHERE mpm.owner_user_id = @owner ORDER BY mpm.updated_at DESC, mpm.created_at DESC";
            return DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@owner", ownerUserId));
        }

        public static DataTable GetGroups(int ownerUserId)
        {
            EnsureTables();
            var sql = "SELECT COALESCE(display_name,'(기본)') AS group_name, COUNT(*) AS target_count, GROUP_CONCAT(target_user_id) AS target_ids, MIN(created_at) AS first_created, MAX(updated_at) AS last_updated, SUM(CASE WHEN photo IS NOT NULL THEN 1 ELSE 0 END) AS photo_rows FROM multi_profile_map WHERE owner_user_id = @owner GROUP BY display_name ORDER BY last_updated DESC, first_created DESC";
            return DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@owner", ownerUserId));
        }

        public static DataTable GetCompanyUsersExceptOwner(int ownerUserId)
        {
            var dt = DBManager.Instance.ExecuteDataTable("SELECT company_id FROM users WHERE id = @id", new MySqlParameter("@id", ownerUserId));
            int companyId = 0;
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value) companyId = Convert.ToInt32(dt.Rows[0][0]);
            if (companyId <= 0) return new DataTable();
            var sql = "SELECT id, COALESCE(full_name, email) AS name FROM users WHERE company_id = @cid AND id <> @owner ORDER BY name";
            return DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@cid", companyId), new MySqlParameter("@owner", ownerUserId));
        }

        public static (string DisplayName, byte[] Photo, int TargetUserId) GetMapping(int mappingId)
        {
            EnsureTables();
            var dt = DBManager.Instance.ExecuteDataTable("SELECT display_name, photo, target_user_id FROM multi_profile_map WHERE id = @id", new MySqlParameter("@id", mappingId));
            if (dt != null && dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                string name = row["display_name"] == DBNull.Value ? null : row["display_name"].ToString();
                byte[] photo = row["photo"] == DBNull.Value ? null : (byte[])row["photo"];
                int target = row["target_user_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["target_user_id"]);
                return (name, photo, target);
            }
            return (null, null, 0);
        }

        public static (string DisplayName, byte[] Photo, List<int> Targets) GetGroup(string groupDisplayName, int ownerUserId)
        {
            EnsureTables();
            string dn = groupDisplayName == "(기본)" ? null : groupDisplayName;
            var dt = DBManager.Instance.ExecuteDataTable("SELECT display_name, photo, target_user_id FROM multi_profile_map WHERE owner_user_id=@owner AND ((@dn IS NULL AND display_name IS NULL) OR display_name=@dn)", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@dn", (object)dn ?? DBNull.Value));
            var targets = new List<int>();
            byte[] photo = null; string disp = dn;
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    if (r["target_user_id"] != DBNull.Value) targets.Add(Convert.ToInt32(r["target_user_id"]));
                    if (photo == null && r["photo"] != DBNull.Value) photo = (byte[])r["photo"];
                    if (disp == null && r["display_name"] != DBNull.Value) disp = r["display_name"].ToString();
                }
            }
            return (disp, photo, targets);
        }

        public static void SaveGroup(int ownerUserId, string groupDisplayName, byte[] photoBytes, IEnumerable<int> selectedTargets)
        {
            EnsureTables();
            string dn = groupDisplayName == "(기본)" ? null : groupDisplayName;
            var set = new HashSet<int>(selectedTargets ?? Array.Empty<int>());
            var existing = DBManager.Instance.ExecuteDataTable("SELECT id, target_user_id FROM multi_profile_map WHERE owner_user_id=@owner AND ((@dn IS NULL AND display_name IS NULL) OR display_name=@dn)", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@dn", (object)dn ?? DBNull.Value));
            var existingTargets = new HashSet<int>();
            if (existing != null)
            {
                foreach (DataRow r in existing.Rows)
                {
                    if (r["target_user_id"] != DBNull.Value) existingTargets.Add(Convert.ToInt32(r["target_user_id"]));
                }
            }
            foreach (var t in existingTargets)
            {
                if (!set.Contains(t))
                {
                    DBManager.Instance.ExecuteNonQuery("DELETE FROM multi_profile_map WHERE owner_user_id=@owner AND target_user_id=@target AND ((@dn IS NULL AND display_name IS NULL) OR display_name=@dn)", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@target", t), new MySqlParameter("@dn", (object)dn ?? DBNull.Value));
                }
            }
            foreach (var t in set)
            {
                try
                {
                    DBManager.Instance.ExecuteNonQuery("INSERT INTO multi_profile_map(owner_user_id,target_user_id,display_name,photo) VALUES(@owner,@target,@dn,@photo)", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@target", t), new MySqlParameter("@dn", (object)dn ?? DBNull.Value), new MySqlParameter("@photo", photoBytes == null ? (object)DBNull.Value : photoBytes));
                }
                catch
                {
                    DBManager.Instance.ExecuteNonQuery("UPDATE multi_profile_map SET display_name=@dn, photo=@photo WHERE owner_user_id=@owner AND target_user_id=@target", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@target", t), new MySqlParameter("@dn", (object)dn ?? DBNull.Value), new MySqlParameter("@photo", photoBytes == null ? (object)DBNull.Value : photoBytes));
                }
            }
        }

        public static void DeleteGroup(int ownerUserId, string groupDisplayName)
        {
            EnsureTables();
            string dn = groupDisplayName == "(기본)" ? null : groupDisplayName;
            DBManager.Instance.ExecuteNonQuery("DELETE FROM multi_profile_map WHERE owner_user_id=@owner AND ((@dn IS NULL AND display_name IS NULL) OR display_name=@dn)", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@dn", (object)dn ?? DBNull.Value));
        }

        public static void DeleteMapping(int ownerUserId, int mappingId)
        {
            EnsureTables();
            DBManager.Instance.ExecuteNonQuery("DELETE FROM multi_profile_map WHERE owner_user_id=@owner AND id=@id", new MySqlParameter("@owner", ownerUserId), new MySqlParameter("@id", mappingId));
        }

        // 호환성용: 기존 SaveMapping 호출을 그룹 저장으로 위임
        public static void SaveMapping(int ownerUserId, int? mappingId, string displayName, byte[] photoBytes, IEnumerable<int> selectedTargets)
        {
            // mappingId는 단일 row 편집 의미였지만 단일 테이블 구조에서는 display_name 그룹 전체 편집으로 통합
            var gName = string.IsNullOrWhiteSpace(displayName) ? "(기본)" : displayName;
            SaveGroup(ownerUserId, gName, photoBytes, selectedTargets);
        }

        public static string GetDisplayNameForViewer(int ownerUserId, int viewerUserId)
        {
            EnsureTables();
            var sql = "SELECT COALESCE(mpm.display_name, u.full_name, u.email) AS name FROM users u LEFT JOIN multi_profile_map mpm ON mpm.owner_user_id = u.id AND mpm.target_user_id = @viewer WHERE u.id = @owner";
            var obj = DBManager.Instance.ExecuteScalar(sql, new MySqlParameter("@viewer", viewerUserId), new MySqlParameter("@owner", ownerUserId));
            return obj == null || obj == DBNull.Value ? null : obj.ToString();
        }

        public static byte[] GetProfileImageForViewer(int ownerUserId, int viewerUserId)
        {
            EnsureTables();
            var sql = "SELECT COALESCE(mpm.photo, u.profile_image) AS photo FROM users u LEFT JOIN multi_profile_map mpm ON mpm.owner_user_id = u.id AND mpm.target_user_id = @viewer WHERE u.id = @owner";
            var dt = DBManager.Instance.ExecuteDataTable(sql, new MySqlParameter("@viewer", viewerUserId), new MySqlParameter("@owner", ownerUserId));
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                return (byte[])dt.Rows[0][0];
            return null;
        }
    }
}
