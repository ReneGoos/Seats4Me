using System.Collections.Generic;
using AutoMapper;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Result;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models
{
    public class Seats4MeProfile : Profile
    {
        public Seats4MeProfile()
        {
            CreateMap<Show, ShowOutputModel>();
            CreateMap<TimeSlot, TimeSlotOutputModel>();
            CreateMap<ShowInputModel, Show>();
            CreateMap<TicketResult, TicketOutputModel>();
        }
    }
}