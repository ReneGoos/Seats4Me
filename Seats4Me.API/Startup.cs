using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seats4Me.API.Model;
using Seats4Me.Data.Model;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Seats4Me.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string conn = Configuration.GetConnectionString("Seats4MeConnection");
            if (conn.Contains("%USERPROFILE%"))
            {
                var path = Configuration.GetSection("USERPROFILE").Value;
                conn = conn.Replace("%USERPROFILE%", path);
            }

            services.AddDbContext<TheatreContext>(options =>
                options.UseSqlServer(conn)
                        .EnableSensitiveDataLogging());
            
            services.AddTransient<ShowsRepository, ShowsRepository>();
            services.AddTransient<TicketsRepository, TicketsRepository>();

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TheatreAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                var context = serviceProvider.GetService<TheatreContext>();
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                AddTestData(context);
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheatreAPI V1");
            });

            app.UseMvc();
        }

        private static void AddTestData(TheatreContext context)
        {
            if (context.Shows.Any())
                return;

            var today = DateTime.Today;
            var dateTimeEvening = new DateTime(today.Year, today.Month, today.Day, 20, 0, 0);
            var dateTimeAfternoon = new DateTime(today.Year, today.Month, today.Day, 14, 30, 0);
            context.Shows.Add(new Show()
            {
                Name = "Twools, Scapino Ballet",
                Title = "Twools",
                Description = "TWOOLS is geen gewone dansvoorstelling, maar een opwindende high-speed choreografische achtbaan.",
                RegularPrice = 27.50M,
                RegularDiscountPrice = 15.0M,
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = dateTimeEvening,
                        Hours = 2
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(1),
                        Hours = 2
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(2),
                        Hours = 2
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeAfternoon.AddDays(5),
                        Hours = 2
                    }
                }
            });
            context.Shows.Add(new Show()
            {
                Name = "Woef Side Story (8+), RO Theater",
                Title = "Woef Side Story",
                Description = "De familiehit Woef Side Story (8+) is een Romeo en Julia op z’n hondjes. De stoere straathond Toto en de bloedmooie rashond Marina zijn verliefd. Maar in een wereld waarin iedereen in hondenhokjes is ingedeeld, mag hun liefde niet bestaan. Is de liefde van Toto en Marina sterk genoeg?",
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day =dateTimeEvening.AddDays(8),
                        Hours =  1.5
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(10),
                        Hours =  1.5
                    }
                }
            });

            for (var row = 1; row <= 15; row++)
            for (var chair = 1; chair <= 6; chair++)
                context.Seats.Add(new Seat()
                {
                    Row = row,
                    Chair = chair
                });
            context.SaveChanges();
        }
    }
}
