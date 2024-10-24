using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class ReferenceInputDto
    {
        [Required]
        public string Name { get; set; }
    }
}
