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
        /// </summary>
        public static DataTable LoadVisibleEmployees(int viewerId, int companyId, string keyword = null)
        {
            // 1) 이 사용자가 가진 보기 권한이 있는지 먼저 확인
            var permDt = DBManager.Instance.ExecuteDataTable(
                "SELECT dept_id FROM user_view_permission WHERE viewer_user_id = @uid",
                new MySqlParameter("@uid", viewerId));

            bool hasPermissionLimit = permDt.Rows.Count > 0;

            var sql = new StringBuilder();
            var pars = new List<MySqlParameter>
            {
                new MySqlParameter("@cid", companyId),
                new MySqlParameter("@viewerId", viewerId)
            };

            // team_id를 포함하도록 컬럼 확장
            sql.Append("SELECT u.id, COALESCE(u.full_name, u.email) AS name, u.email, ");
            sql.Append("u.department_id, u.team_id, d.name AS department ");
            sql.Append("FROM users u ");
            sql.Append("LEFT JOIN departments d ON d.id = u.department_id ");

            if (!hasPermissionLimit)
            {
                //권한 레코드가 없으면 → 제한 없음 = 회사 내 전체 직원
                sql.Append("WHERE u.company_id = @cid ");
            }
            else
            {
                //권한 레코드가 있으면 → 허용된 부서 직원만
                sql.Append("JOIN user_view_permission p ");
                sql.Append("ON p.viewer_user_id = @viewerId ");
                sql.Append("AND p.dept_id = u.department_id ");
                sql.Append("WHERE u.company_id = @cid ");
            }

            // 검색어(이름/이메일) 옵션
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
