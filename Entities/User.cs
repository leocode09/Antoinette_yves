using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        [MaxLength(125)]
        public string Username { get; set; }
        [MaxLength(125)]
        public string Names { get; set; }
        [MaxLength(125)]
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        
    }
}
