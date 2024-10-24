using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class Barber
    {
        public int Id { get; set; }
        [MaxLength(125)]
        public string Names { get; set; }
        [MaxLength(15)]
        public string Phone { get; set; }
        [MaxLength(125)]
        public string Email { get; set; }
        public int? PartnerId { get; set; }
        public Partner Partner { get; set; }
        public DateTime CreatedOnDate { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }
        public Guid? CreatedById { get; set; }
        public User CreatedBy { get; set; }
    }
}
