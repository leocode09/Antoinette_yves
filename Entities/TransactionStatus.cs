using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class TransactionStatus
    {
        public int Id { get; set; }
        [MaxLength(125)]
        public string Name { get; set; }
    }
}
