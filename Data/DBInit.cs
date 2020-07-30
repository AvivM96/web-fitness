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
            if (!context.AspNetUsers.Any(t => t.IsTrainer))
            {
                var t = new ApplicationUser[]
                {
                new ApplicationUser{FirstName="Gil", LastName="Semo", PhoneNumber="050000000", Email="gil@mail.com", Gender="male", Address="Where", City="There"},
                new ApplicationUser{FirstName="Gil2", LastName="Semo2", PhoneNumber="050000000", Email="gil2@mail.com", Gender="male", Address="Where", City="There"},
                };
                foreach (ApplicationUser a in t)
                {
                    context.AspNetUsers.Add(a);
                }
                context.SaveChanges();
            }
            if (!context.Meetings.Any())
            {
                var m = new Meeting[]
                {
               // new Meeting{MeetNum=1, Trainer=context.Trainers.First(), TrainType=context.TrainingTypes.First(), MeetDate=DateTime.Now},
                //new Meeting{MeetNum=2, Trainer=context.Trainers.First(), TrainType=context.TrainingTypes.First(), MeetDate=DateTime.Now},
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