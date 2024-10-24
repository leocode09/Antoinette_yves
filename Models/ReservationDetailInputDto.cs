using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ReservationDetailInputDto
    {
        [Required]
        public int PartnerServiceId { get; set; }
        [Required]
        public DateTime AppointmentTime { get; set; }
        public int? BarberId { get; set; }
        public int Price { get; set; }
    }
}
