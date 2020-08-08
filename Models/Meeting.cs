using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace web_fitness.Models
{
public class Meeting : IValidatableObject
    {
        [Key]
        public int MeetID { get; set; }

        [ForeignKey("TrainingTypeID")]
        public int TrainingTypeID { get; set; }

        [ForeignKey("UserId")]
        public string TrainerID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime MeetDate { get; set; }

        [Required]
        [Range(0, 1000)]
        public int Price { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (MeetDate < DateTime.Now)
            {
                results.Add(new ValidationResult("Can not create meeting with past date", new[] { "MeetDate" }));
            }

            return results;
        }

        public ApplicationUser Trainer { get; set; }
        public TrainingType TrainType { get; set; }
    }
}
