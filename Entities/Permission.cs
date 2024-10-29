using System;
using System.Collections.Generic;

namespace WeCareWebApp.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public DateTime? ModifiedOnDate { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        public Permission()
        {
            RolePermissions = new HashSet<RolePermission>();
        }
    }
} 