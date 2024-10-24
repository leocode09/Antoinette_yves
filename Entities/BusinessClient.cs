using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class BusinessClient
    {
        [MaxLength(10)]
        public string Id { get; set; }
        [MaxLength(125)]
        public string Name { get; set; }
        [MaxLength(25)]
        public string SerialNumber { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
