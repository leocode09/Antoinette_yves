using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class Reservation
    {
        [MaxLength(11)]
        public string Id { get; set; }
        public string BusinessClientId { get; set; }
        public BusinessClient BusinessClient { get; set; }

        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }

        public DateTime ReservationDate { get; set; }
        public int Amount { get; set; }
        [StringLength(6)]
        public string Code { get; set; }

        public int? ReservationStatusId { get; set; }
        public ReservationStatus ReservationStatus { get; set; }

        public ICollection<ReservationDetail> Details { get; set; }
        
    }
}
