using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class ClientMovement
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public BusinessClient Client { get; set; }

        public string OriginLatitude { get; set; }
        public string OriginLongitude { get; set; }

        public string DestinationLatitude { get; set; }
        public string DestinationLongitude { get; set; }
        public DateTime CreatedOnDate { get; set; }

    }
}
