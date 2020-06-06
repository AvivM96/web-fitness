using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace web_fitness.Models
{
    public class Trainer
    {

        [Key]
        [ScaffoldColumn(false)]
        public int TrainerId { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Trainer Mail")]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "Trainer Name")]
        public string TrainerName { get; set; }

        [Display(Name = "Trainer Phone")]
        public string TrainerPhone { get; set; }

        [Display(Name = "Trainer Gender")]
        public string TrainerGender { get; set; }

        [Display(Name = "Trainer Address")]
        [StringLength(60, MinimumLength = 5)]
        [Required]
        public string Address { get; set; }

        [Display(Name = "Trainer City")]
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string City { get; set; }


        public ICollection<Meeting> Meeting { get; set; }





    }
}
