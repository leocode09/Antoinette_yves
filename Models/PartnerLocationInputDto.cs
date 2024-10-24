using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerLocationInputDto
    {
        public int PartnerId { get; set; }
        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
    }
}
