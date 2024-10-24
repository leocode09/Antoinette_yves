using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class ReservationDetail
    {

        [MaxLength(13)]
        public string Id { get; set; }
        public string ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int PartnerServiceId { get; set; }
        public PartnerService PartnerService { get; set; }
        public DateTime ReservationDate { get; set; }
        [MaxLength(6)]
        public string Code { get; set; }
        public bool IsServed { get; set; }
        public int? BarberId { get; set; }
        public Barber Barber { get; set; }
        public int Amount { get; set; }
    }
}
