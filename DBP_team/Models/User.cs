using System;

namespace DBP_team.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public int? DepartmentId { get; set; }
        public int? TeamId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
    }
}
