using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ReservationDto
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Client { get; set; }

        public int? PartnerId { get; set; }
        public string Partner { get; set; }
        public int Amount { get; set; }
        
        public int? ReservationStatusId { get; set; }
        public string ReservationStatus { get; set; }
        public List<ReservationDetailDto> Details { get; set; }
    }
}
