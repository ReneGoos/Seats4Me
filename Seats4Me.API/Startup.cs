using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Seats4Me.API.Models;
using AutoMapper;
using Seats4Me.API.Services;

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

            services.AddScoped<IShowsService, ShowsService>();
            services.AddScoped<ITicketsService, TicketsService>();
            services.AddScoped<ITimeSlotsService, TimeSlotsService>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IShowsRepository, ShowsRepository>();
            services.AddScoped<ITimeSlotsRepository, TimeSlotsRepository>();
            services.AddScoped<ITimeSlotSeatsRepository, TimeSlotSeatsRepository>();
            services.AddScoped<IUsersRepository, UsersRepository>();

            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Signing:Key"]));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options =>
                    {
                        options.TokenValidationParameters =
                            new TokenValidationParameters()
                            {
                                ValidateIssuerSigningKey = true,
                                IssuerSigningKey = key,

                                ValidateIssuer = true,
                                ValidIssuer = Configuration["Signing:Issuer"],

                                ValidateAudience = true,
                                ValidAudience = Configuration["Signing:Audience"],

                                ValidateLifetime = true,

                                ClockSkew = TimeSpan.Zero
                            };
                    });

            /*
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events.OnRedirectToLogin += OnRedirectToLogin,
                    options.LoginPath = "/Account/LogIn";
                    options.LogoutPath = "/Account/LogOff";
                });
                */
            /*
            var builder = services.AddIdentityCore<Seats4MeUser>(opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequiredLength = 8;
                    opt.Password.RequireNonAlphanumeric = false;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequireLowercase = true;
                }
            );
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder

            builder.AddRoleValidator<RoleValidator<IdentityRole>>();
            builder.AddRoleManager<RoleManager<IdentityRole>>();
            builder.AddSignInManager<SignInManager<IdentityUser>>();
            */

            services.AddAuthorization(cfg =>
            {
                cfg.AddPolicy("Owner", p => p.RequireClaim("Owner", "True"));
                cfg.AddPolicy("Contributor", p => p.RequireClaim("Contributor", "True"));
                cfg.AddPolicy("Customer", p => p.RequireClaim("Customer", "True"));
            });

            services.AddMvcCore(setupAction => { setupAction.ReturnHttpNotAcceptable = false; })
                .AddAuthorization()
                .AddJsonFormatters()
                .AddXmlSerializerFormatters()
                .AddApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "TheatreAPI", Version = "v1" });

                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);
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

                c.DocumentTitle = "Title Documentation";
                c.DocExpansion(DocExpansion.None);
            });

            app.UseAuthentication();

            Mapper.Initialize(cfg => cfg.AddProfile<Seats4MeProfile>());

            app.UseMvc();
        }

        private static void AddTestData(TheatreContext context)
        {
            if (context.Shows.Any())
                return;

            context.Seats4MeUsers.AddRange(
                new List<Seats4MeUser>()
                {
                    new Seats4MeUser()
                    {
                        Email = "guest@seats4me.com",
                        Name = "Guest",
                        Roles = "guest",
                        Password = "password"
                    },
                    new Seats4MeUser()
                    {
                        Email = "admin@seats4me.com",
                        Name = "Admin",
                        Roles = "owner,contributor,customer",
                        Password = "password"
                    },
                    new Seats4MeUser()
                    {
                        Email = "rene@seats4me.com",
                        Name = "René",
                        Roles = "contributor,customer",
                        Password = "password"
                    },
                    new Seats4MeUser()
                    {
                        Email = "manuel@seats4me.com",
                        Name = "Manuel",
                        Roles = "customer",
                        Password = "password"
                    }
                });

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
                        Day = dateTimeEvening.AddDays(-7),
                        Hours = 2
                    },
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
                RegularPrice = 17.50M,
                RegularDiscountPrice = 12.5M,
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(8),
                        Hours =  1.5
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(10),
                        Hours =  1.5
                    }
                }
            });

            context.Shows.Add(new Show()
            {
                Name = "OUDFIT, Theater Zuidplein",
                Title = "OUDFIT",
                Description = "OUDFIT gaat over ouderen die zich staande proberen te houden in een steeds jonger, hipper en sneller Rotterdam. Een oergezellige middag!",
                RegularPrice = 15.0M,
                RegularDiscountPrice = 10.0M,
                TimeSlots = new List<TimeSlot>()
                {
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(31),
                        Hours =  1.5
                    },
                    new TimeSlot()
                    {
                        Day = dateTimeEvening.AddDays(31),
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

            var seat = context.Seats.FirstOrDefault(s => s.Row == 1 && s.Chair == 1);
            var timeSlot = context.TimeSlots.FirstOrDefault(ts => ts.Day == dateTimeEvening);
            var user = context.Seats4MeUsers.FirstOrDefault(u => u.Email.Equals("rene@seats4me.com"));

            if (seat != null && timeSlot != null)
            {
                context.TimeSlotSeats.Add(new TimeSlotSeat()
                {
                    SeatId = seat.Id,
                    TimeSlotId = timeSlot.Id,
                    Seats4MeUserId = user?.Id ?? -1,
                    Paid = true,
                    Price = 15
                });
            }

            context.SaveChanges();
        }
    }
}
