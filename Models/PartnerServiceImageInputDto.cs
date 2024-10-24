using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class PartnerServiceImageInputDto
    {
        public int PartnerServiceId { get; set; }
        public IFormFile ImageData { get; set; }
    }
}
