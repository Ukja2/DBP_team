using System;

namespace DBP_team.Models
{
    public class User
    {
        public int Id { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; } // DB에 있는 회사명을 전달/저장하기 위한 속성 추가
        public int? DepartmentId { get; set; }
        public int? TeamId { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsFavorite { get; set; } // 즐겨찾기 여부 표시
    }
}
