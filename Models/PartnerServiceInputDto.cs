using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerServiceInputDto
    {
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public int BusinessServiceId { get; set; }
        [Required]
        public int Price { get; set; }
        public int? Seats { get; set; }
        public int? MinDuration { get; set; }
    }
}
