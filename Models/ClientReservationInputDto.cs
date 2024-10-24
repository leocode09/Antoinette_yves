using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ClientReservationInputDto
    {

        public string SerialNumber { get; set; }
        [MaxLength(125), Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }

        public int Amount { get; set; }

        public List<ReservationDetailInputDto> Details { get; set; }
    }
}
