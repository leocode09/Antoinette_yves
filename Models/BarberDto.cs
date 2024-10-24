using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Models
{
    public class BarberDto
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public string Phone { get; set; }
        public int? PartnerId { get; set; }
        public string Partner { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
