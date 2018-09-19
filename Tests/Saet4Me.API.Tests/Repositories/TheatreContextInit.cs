using System;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Seats4Me.Data.Model;

namespace Seats4Me.API.Tests.Repositories
{
    public static class TheatreContextInit
    {
        public static void AddTestData(TheatreContext context)
        {
            var today = DateTime.Today;
            var dateTimeEvening = new DateTime(today.Year, today.Month, today.Day, 20, 0, 0);
            var dateTimeAfternoon = new DateTime(today.Year, today.Month, today.Day, 14, 30, 0);
            context.Shows.Add(new Show
                              {
                                  Name = "Hamlet vs Hamlet",
                                  Title = "Hamlet vs. Hamlet",
                                  Description =
                                      "Guy Cassiers, Tom Lanoye en het stuk der stukken: Shakespeares Hamlet. Cassiers regisseerde deze bewerking met een grote cast, samengesteld uit de ensembles van Toneelhuis en Toneelgroep Amsterdam.",
                                  RegularPrice = 27.50M,
                                  RegularDiscountPrice = 15.0M,
                                  TimeSlots = new List<TimeSlot>
                                              {
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening.AddDays(-2),
                                                      Hours = 2
                                                  },
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening,
                                                      Hours = 2
                                                  },
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening.AddDays(1),
                                                      Hours = 2
                                                  },
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening.AddDays(2),
                                                      Hours = 2
                                                  },
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeAfternoon.AddDays(5),
                                                      Hours = 2
                                                  }
                                              }
                              });

            context.Shows.Add(new Show
                              {
                                  Name = "Branden",
                                  Title = "Branden (Incendies)",
                                  Description = "'Branden' van de Libanese toneelschrijver Wajdi Mouawad, in regie van Alize Zandwijk, werd begin 2010 voor het eerst in Nederland opgevoerd door het Ro Theater.",
                                  TimeSlots = new List<TimeSlot>
                                              {
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening.AddDays(8),
                                                      Hours = 1.5
                                                  },
                                                  new TimeSlot
                                                  {
                                                      Day = dateTimeEvening.AddDays(10),
                                                      Hours = 1.5
                                                  }
                                              }
                              });

            for (var row = 1; row <= 15; row++)
            {
                for (var chair = 1; chair <= 6; chair++)
                {
                    context.Seats.Add(new Seat
                                      {
                                          Row = row,
                                          Chair = chair
                                      });
                }
            }

            context.SaveChanges();
        }

        public static TheatreContext InitializeContextInMemoryDb(string name)
        {
            var builder = new DbContextOptionsBuilder<TheatreContext>().UseInMemoryDatabase(name);
            var context = new TheatreContext(builder.Options);

            return context;
        }
    }
}