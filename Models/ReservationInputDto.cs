using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ReservationInputDto
    {
        [Required]
        public string Phone { get; set; }
        public int Amount { get; set; }

        public List<ReservationDetailInputDto> Details { get; set; }
    }
}
