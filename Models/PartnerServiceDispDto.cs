using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerServiceDispDto
    {
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int PartnerId { get; set; }
        public string Partner { get; set; }
        public byte[] ServiceImage { get; set; }
    }
}
