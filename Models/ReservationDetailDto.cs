using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ReservationDetailDto
    {
        public string Id { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public int PartnerId { get; set; }
        public string Partner { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsServed { get; set; }
        public string SerialNumber { get; set; }

        public int? BarberId { get; set; }
        public string Barber { get; set; }
        public int Amount { get; set; }
    }
}
