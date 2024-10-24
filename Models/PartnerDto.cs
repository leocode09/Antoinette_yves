using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string MomoCode { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid UserId { get; set; }
        public Guid? CreatedById { get; set; }
        public string CreatedBy { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
    }
}
