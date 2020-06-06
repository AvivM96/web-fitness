﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace web_fitness.Models
{
    public class TrainingType
    {
        [Key]
        [ScaffoldColumn(false)]
        public int TrainingTypeId { get; set; }

        [Required]
        [Display(Name = "Training Name")]
        [StringLength(60, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Training Target")]
        [Required]
        [StringLength(60, MinimumLength = 2)]
        public string Target { get; set; }


        public ICollection<Meeting> Meeting { get; set; }




    }
}
