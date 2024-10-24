using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerInputDto
    {
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Location { get; set; }
        [MaxLength(125)]
        public string Email { get; set; }
        [MaxLength(10)]
        public string Phone { get; set; }
        [MaxLength(8)]
        public string MomoCode { get; set; }

        public Guid? UserId { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }
    }

}
