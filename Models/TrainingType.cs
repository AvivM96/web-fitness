using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_fitness.Models
{
    public class TrainingType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Target { get; set; }




    }
}
