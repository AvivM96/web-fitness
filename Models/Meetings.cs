using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace web_fitness.Models
{
    public class Meetings
    {

        modelBuilder.Entity<Participants>()
        .HasKey(p => new { p.ActivityId , p.ParticipantId
    });


        public int MeetID { get; set; }
        [Column(Order = 0), Key, ForeignKey("TrainerId")]
        public int TrainerId { get; set; }
        [Column(Order = 1), Key, ForeignKey("TypeId")]
        public int TypeId { get; set; }

        [Column(Order = 2), Key, DataType(DataType.Date)]
        public DateTime MeetDate { get; set; }


        public Trainers Trainer { get; set; }
        public TrainingType TrainType { get; set; }
    }
}
