using AutoMapper;

using Seats4Me.API.Common;
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
            CreateMap<TimeSlot, TimeSlotOutputModel>().ForMember(dest => dest.Week, opt => opt.MapFrom(src => src.Day.Week()));
            CreateMap<ShowInputModel, Show>();
            CreateMap<TimeSlotInputModel, TimeSlot>();
            CreateMap<TicketInputModel, TimeSlotSeat>();

            CreateMap<TimeSlotSeat, TicketResult>()
                .ForMember(dest => dest.TimeSlot, opt => opt.MapFrom(src => src.TimeSlot))
                .ForMember(dest => dest.Seat, opt => opt.MapFrom(src => src.Seat))
                .ForMember(dest => dest.TimeSlotSeat, opt => opt.MapFrom(src => src));

            CreateMap<TicketResult, TicketOutputModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TimeSlot.Show.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.TimeSlot.Show.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.TimeSlot.Show.Description))
                .ForMember(dest => dest.RegularPrice, opt => opt.MapFrom(src => src.TimeSlot.Show.RegularPrice))
                .ForMember(dest => dest.RegularDiscountPrice, opt => opt.MapFrom(src => src.TimeSlot.Show.RegularDiscountPrice))
                .ForMember(dest => dest.PromoPrice, opt => opt.MapFrom(src => src.TimeSlot.PromoPrice))
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.TimeSlot.Day))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Seat.Row))
                .ForMember(dest => dest.Chair, opt => opt.MapFrom(src => src.Seat.Chair))
                .ForMember(dest => dest.TimeSlotSeatId, opt => opt.MapFrom(src => src.TimeSlotSeat.Id))
                .ForMember(dest => dest.Reserved, opt => opt.MapFrom(src => src.TimeSlotSeat.Reserved))
                .ForMember(dest => dest.Paid, opt => opt.MapFrom(src => src.TimeSlotSeat.Paid))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.TimeSlotSeat.Price));
        }
    }
}