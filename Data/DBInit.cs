using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using web_fitness.Models;

namespace web_fitness.Data
{
    public  class DBInit
    { 

        public static void Initialize(fitnessdataContext context)
        {
            context.Database.EnsureCreated();
            if (!context.TrainingTypes.Any())
            {
                var tt = new TrainingType[]
                {
                new TrainingType{Name="TRX",Target="Working on Core muscels"},
                new TrainingType{Name= "Zoomba",Target="Body shaping quickly and efficiently"},
                };
                foreach (TrainingType a in tt)
                {
                    context.TrainingTypes.Add(a);
                }
                context.SaveChanges();
            }
            if (!context.Trainers.Any())
            {
                var t = new Trainer[]
                {
                new Trainer{TrainerName="Gil", TrainerPhone="050000000", Mail="gil@mail.com", TrainerGender="male", Address="Where", City="There"},
                new Trainer{TrainerName="Gil2", TrainerPhone="050000002", Mail="gil2@mail.com", TrainerGender="male", Address="Where", City="There"},
                };
                foreach (Trainer a in t)
                {
                    context.Trainers.Add(a);
                }
                context.SaveChanges();
            }
            if (!context.Meetings.Any())
            {
                var m = new Meeting[]
                {
                new Meeting{MeetNum=1, Trainer=context.Trainers.First(), TrainType=context.TrainingTypes.First(), MeetDate=DateTime.Now},
                new Meeting{MeetNum=2, Trainer=context.Trainers.First(), TrainType=context.TrainingTypes.First(), MeetDate=DateTime.Now},
                };
                foreach (Meeting a in m)
                {
                    context.Meetings.Add(a);
                }
                context.SaveChanges();
            }
        }
    }
}