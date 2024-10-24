using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class UserInputDto
    {
        [MaxLength(125),Required]
        public string Username { get; set; }
        [MaxLength(125),Required]
        public string Names { get; set; }
        [Required]
        public int RoleId { get; set; }
    }
}
