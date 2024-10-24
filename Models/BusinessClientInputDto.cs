using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class BusinessClientInputDto
    {
        [MaxLength(10),Required,MinLength(10)]
        public string Phone { get; set; }
        [MaxLength(125),Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string SerialNumber { get; set; }
    }
}
