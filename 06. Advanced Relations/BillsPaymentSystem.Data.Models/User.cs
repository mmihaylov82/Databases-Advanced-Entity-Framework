using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();
    }
}
