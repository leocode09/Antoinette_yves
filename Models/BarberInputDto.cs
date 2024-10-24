using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class BarberInputDto
    {
        [Required]
        public string Names { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Email { get; set; }
        public int? PartnerId { get; set; }
        public Guid? UserId { get; set; }
    }
}
