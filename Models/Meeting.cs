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
public class Meeting
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
        [Range(0, 10000000000)]
        public int Price { get; set; }



        public ApplicationUser Trainer { get; set; }
        public TrainingType TrainType { get; set; }
    }
}
