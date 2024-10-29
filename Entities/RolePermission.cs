using System;

namespace WeCareWebApp.Entities
{
    public class RolePermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int PermissionId { get; set; }
        public DateTime CreatedOnDate { get; set; }
        
        public virtual Role Role { get; set; }
        public virtual Permission Permission { get; set; }
    }
} 