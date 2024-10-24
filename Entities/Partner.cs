using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class Partner
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Location { get; set; }
        [MaxLength(125)]
        public string Email { get; set; }
        [MaxLength(10)]
        public string Phone { get; set; }
        [MaxLength(8)]
        public string MomoCode { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }

    }
}
