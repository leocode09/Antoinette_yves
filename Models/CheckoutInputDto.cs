using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class CheckoutInputDto
    {
        public string Phone { get; set; }
        public string ReservationId { get; set; }
        public int Amount { get; set; }
    }
}
