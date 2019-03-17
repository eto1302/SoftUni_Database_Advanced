using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BillsPaymentSystem.Models.Enums;

namespace BillsPaymentSystem.Models
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public PaymentType Type { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        [ForeignKey("BankAccount")]
        [Xor("CreditCardId")]
        public int? BankAccountId { get; set; }
        
        [Required]
        [ForeignKey("CreditCard")]
        [Xor("BankAccountId")]
        public int? CreditCardId { get; set; }
    }
}
