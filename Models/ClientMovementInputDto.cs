using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ClientMovementInputDto
    {
        public string ClientId { get; set; }

        public string OriginLatitude { get; set; }
        public string OriginLongitude { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
    }
}
