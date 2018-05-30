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
using System.Globalization;

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
            services.AddDbContext<TheatreContext>(b => b.UseInMemoryDatabase(new Guid().ToString()));
            services.AddTransient<ShowsRepository, ShowsRepository>();
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
                AddTestData(context);
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TheatreAPI V1");
            });

            app.UseMvc();
        }

        private void AddTestData(TheatreContext context)
        {
            context.Shows.Add(new Show()
            {
                ShowId = 1,
                Name = "Hamlet vs. Hamlet"
            });
            context.Shows.Add(new Show()
            {
                ShowId = 2,
                Name = "Branden"
            });

            context.TimeSlots.AddRange(new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        TimeSlotId = 1,
                        Start = Convert.ToDateTime("01-06-2018 20:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("01-06-2018 22:00", CultureInfo.CurrentCulture),
                        ShowId = 1
                    },
                    new TimeSlot()
                    {
                        TimeSlotId = 2,
                        Start = Convert.ToDateTime("02-06-2018 20:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("02-06-2018 22:00", CultureInfo.CurrentCulture),
                        ShowId = 1
                    },
                    new TimeSlot()
                    {
                        TimeSlotId = 3,
                        Start = Convert.ToDateTime("03-06-2018 14:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("03-06-2018 16:00", CultureInfo.CurrentCulture),
                        ShowId = 1
                    },
                    new TimeSlot()
                    {
                        TimeSlotId = 4,
                        Start = Convert.ToDateTime("03-06-2018 20:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("03-06-2018 22:00", CultureInfo.CurrentCulture),
                        ShowId = 1
                    },
                    new TimeSlot()
                    {
                        TimeSlotId = 5,
                        Start = Convert.ToDateTime("08-06-2018 20:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("08-06-2018 22:00", CultureInfo.CurrentCulture),
                        ShowId = 2
                    },
                    new TimeSlot()
                    {
                        TimeSlotId = 6,
                        Start = Convert.ToDateTime("09-06-2018 20:00", CultureInfo.CurrentCulture),
                        End =  Convert.ToDateTime("09-06-2018 22:00", CultureInfo.CurrentCulture),
                        ShowId = 2
                    }
            });
            context.SaveChanges();
        }
    }
}
