using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_fitness.Models
{
    public class Customer
    {
        [Key]
        [ScaffoldColumn(false)]
        public int CustomerId { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Customer Mail")]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Customer Phone")]
        public string CustomerPhone { get; set; }

        [Display(Name = "Customer Gender")]
        public string CustomerGender { get; set; }

        [Display(Name = "Customer Address")]
        [StringLength(60, MinimumLength = 5)]
        [Required]
        public string CustomerAddress { get; set; }

        [Display(Name = "Customer City")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string CustomerCity { get; set; }

// public ICollection<Meetings> Meeting { get; set; }

    }
}
