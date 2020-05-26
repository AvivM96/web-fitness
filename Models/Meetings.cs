using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;



namespace web_fitness.Models
{
    public class Meetings
    {

        public int MeetID { get; set; }

        [ForeignKey("TrainerId")]
        public int TrainerId { get; set; }
        [ForeignKey("TypeId")]
        public int TypeId { get; set; }
        [DataType(DataType.Date)]
        public DateTime MeetDate { get; set; }


        public Trainers Trainer { get; set; }
        public TrainingType TrainType { get; set; }
    }
}
