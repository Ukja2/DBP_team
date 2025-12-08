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
            // 권한 레코드 존재 여부 확인
            var permDt = DBManager.Instance.ExecuteDataTable(
                "SELECT dept_id, group_code FROM user_view_permission WHERE viewer_user_id = @uid",
                new MySqlParameter("@uid", viewerId));

            bool hasPermissionLimit = permDt.Rows.Count > 0;

            var sql = new StringBuilder();
            var pars = new List<MySqlParameter>
            {
                new MySqlParameter("@cid", companyId),
                new MySqlParameter("@viewerId", viewerId)
            };

            sql.Append("SELECT u.id, COALESCE(u.full_name, u.email) AS name, u.email, ");
            sql.Append("u.department_id, u.team_id, d.name AS department ");
            sql.Append("FROM users u ");
            sql.Append("LEFT JOIN departments d ON d.id = u.department_id ");

            if (!hasPermissionLimit)
            {
                // 제한 없음: 회사 전체
                sql.Append("WHERE u.company_id = @cid ");
            }
            else
            {
                // 권한 제한 있음: dept 허용 또는 team/user 허용에 일치하는 사용자만
                // 팀/사용자 허용을 위해 EXISTS 서브쿼리로 group_code 검사
                sql.Append("WHERE u.company_id = @cid AND (");
                sql.Append(" EXISTS (SELECT 1 FROM user_view_permission p WHERE p.viewer_user_id=@viewerId AND p.dept_id IS NOT NULL AND p.dept_id = u.department_id) ");
                sql.Append(" OR EXISTS (SELECT 1 FROM user_view_permission p WHERE p.viewer_user_id=@viewerId AND p.group_code IS NOT NULL AND ((p.group_code = CONCAT('TEAM:', u.team_id)) OR (p.group_code = CONCAT('USER:', u.id)))) ");
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
