using System;

namespace WeCareWebApp.Entities
{
    public class UserRole
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
} 