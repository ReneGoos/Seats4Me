using System.Collections.Generic;
using AutoMapper;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Result;
using Seats4Me.API.Common;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Models
{
    public class Seats4MeProfile : Profile
    {
        public Seats4MeProfile()
        {
            CreateMap<Show, ShowOutputModel>();
            CreateMap<TimeSlot, TimeSlotOutputModel>()
                .ForMember(dest => dest.Week, opt => opt.MapFrom(src => src.Day.Week()));
            CreateMap<ShowInputModel, Show>();
            CreateMap<TimeSlotInputModel, TimeSlot>();
            CreateMap<TicketResult, TicketOutputModel>();
        }
    }
}