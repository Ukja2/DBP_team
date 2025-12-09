using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace DBP_team.Models
{
    public static class EmployeePermissionService
    {
        /// <summary>
        /// G 기능: 로그인한 사용자(viewerId)가 볼 수 있는 직원 목록만 반환
        /// 권한 테이블(user_view_permission)의 부서/팀/사용자 단위 허용을 모두 반영합니다.
        /// </summary>
        public static DataTable LoadVisibleEmployees(int viewerId, int companyId, string keyword = null)
        {
            var sql = new StringBuilder();
            var pars = new List<MySqlParameter>
            {
                new MySqlParameter("@cid", companyId),
                new MySqlParameter("@viewerId", viewerId)
            };

            // 권한이 하나라도 있으면 제한 모드, 없으면 전체 회사 허용
            var permDt = DBManager.Instance.ExecuteDataTable(
                "SELECT dept_id, group_code FROM user_view_permission WHERE viewer_user_id = @uid",
                new MySqlParameter("@uid", viewerId));
            bool hasPermissionLimit = permDt.Rows.Count > 0;

            sql.Append("SELECT u.id, COALESCE(u.full_name, u.email) AS name, u.email, u.department_id, u.team_id, d.name AS department, t.name AS team ");
            sql.Append("FROM users u ");
            sql.Append("LEFT JOIN departments d ON d.id = u.department_id ");
            sql.Append("LEFT JOIN teams t ON t.id = u.team_id ");

            if (!hasPermissionLimit)
            {
                sql.Append("WHERE u.company_id = @cid ");
            }
            else
            {
                // 제한 모드: 다음 중 하나를 만족하면 표시
                // 1) 부서 허용: dept_id 일치 또는 dept_id=0 이고 u.department_id IS NULL
                // 2) TEAM:x: 해당 팀 허용 (dept와 무관하게 실제 team_id 일치 시 허용)
                // 3) USER:x: 특정 사용자 허용 (id 일치)
                sql.Append("WHERE u.company_id = @cid AND (");
                sql.Append(" EXISTS (SELECT 1 FROM user_view_permission p WHERE p.viewer_user_id=@viewerId AND p.group_code IS NULL AND ((p.dept_id IS NOT NULL AND p.dept_id = u.department_id) OR (p.dept_id = 0 AND u.department_id IS NULL))) ");
                sql.Append(" OR EXISTS (SELECT 1 FROM user_view_permission p WHERE p.viewer_user_id=@viewerId AND p.group_code LIKE 'TEAM:%' AND CAST(SUBSTRING(p.group_code, 6) AS UNSIGNED) = u.team_id) ");
                sql.Append(" OR EXISTS (SELECT 1 FROM user_view_permission p WHERE p.viewer_user_id=@viewerId AND p.group_code LIKE 'USER:%' AND CAST(SUBSTRING(p.group_code, 6) AS UNSIGNED) = u.id) ");
                sql.Append(") ");
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                sql.Append("AND (u.full_name LIKE @kw OR u.email LIKE @kw) ");
                pars.Add(new MySqlParameter("@kw", "%" + keyword + "%"));
            }

            sql.Append("ORDER BY name");

            return DBManager.Instance.ExecuteDataTable(sql.ToString(), pars.ToArray());
        }
    }
}
