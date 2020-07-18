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

        [ForeignKey("TrainerID")]
        public int TrainerID { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime MeetDate { get; set; }

       

        public int MyHash (int a,int b, DateTime c)
        {
            var byteArr = Encoding.ASCII.GetBytes(a.ToString() + c.ToString()+ b.ToString()  );
            var sha = new SHA1CryptoServiceProvider();
            var res = sha.ComputeHash(byteArr);
            return Int32.Parse(res.ToString());
        }


        public Trainer Trainer { get; set; }
        public TrainingType TrainType { get; set; }
    }
}
