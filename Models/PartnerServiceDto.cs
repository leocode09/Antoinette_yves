using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerServiceDto
    {
        public int Id { get; set; }
        public int PartnerId { get; set; }
        public string Partner { get; set; }
        public int ServiceId { get; set; }
        public string Service { get; set; }
        public bool IsActive { get; set; }
        public int Price { get; set; }
        public int? Seats { get; set; }
        public int? MinDuration { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public byte[] ImageData { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
    }
}
