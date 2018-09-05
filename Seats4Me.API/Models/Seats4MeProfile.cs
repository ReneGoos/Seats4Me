using AutoMapper;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seats4Me.API.Models
{
    public class Seats4MeProfile : Profile
    {
        public Seats4MeProfile()
            : base()
        {
            CreateMap<Show, ShowOutputModel>();
            CreateMap<ShowInputModel, Show>();
        }
    }
}
