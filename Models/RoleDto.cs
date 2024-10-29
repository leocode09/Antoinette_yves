using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeCareWebApp.Models;

namespace WeCareWebApp.Models
{
    public class RoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOnDate { get; set; }
    }
}
