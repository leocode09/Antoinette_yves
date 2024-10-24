using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeCareWebApp.Entities
{
    public class PayTransaction
    {
       
        public Guid Id { get; set; }
        public string ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        [MaxLength(125)]
        public string ExternalReferenceNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? TransactionStatusId { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime? CompletedOnDate { get; set; }
        public string X_ReferenceId { get; set; }
        public int Amount { get; set; }
        public string Phone { get; set; }
    }
}
