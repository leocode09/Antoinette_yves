using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class OurServiceDto
    {
        public OurServiceDto()
        {
            Row = new List<PartnerServiceDispDto>();
        }
        public List<PartnerServiceDispDto> Row { get; set; }
    }
}
