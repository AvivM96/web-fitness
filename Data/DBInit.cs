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
            /*
                        if (!context.Sublets.Any())
                        {
                            var sublets = new SubletModel[]
                            {
                            new SubletModel{Title= "Sublet1",ApartmentKey=4, StartDate = new DateTime(2008, 5, 1), EndDate = new DateTime(2008, 5, 7), Price = 4000 },
                            new SubletModel{Title= "Sublet2",ApartmentKey=5, StartDate = new DateTime(2019, 5, 1), EndDate = new DateTime(2019, 5, 5), Price = 2000},
                            };
                            foreach (SubletModel a in sublets)
                            {
                                context.Sublets.Add(a);
                            }
                            context.SaveChanges();
                        }
                    }
                    */
        }
    }
}