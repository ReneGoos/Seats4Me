using AutoMapper;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seats4Me.API.Services
{
    public class TimeSlotsService : ITimeSlotsService
    {
        private readonly ITimeSlotsRepository _timeSlotsRepository;
        private readonly ITimeSlotSeatsRepository _timeSlotSeatRepository;

        public TimeSlotsService(ITimeSlotsRepository timeSlotsRepository, ITimeSlotSeatsRepository timeSlotSeatRepository)
        {
            _timeSlotsRepository = timeSlotsRepository;
            _timeSlotSeatRepository = timeSlotSeatRepository;
        }

        public async Task<List<TimeSlotOutputModel>> GetAsync()
        {
            var timeSlots = await _timeSlotsRepository.GetAsync();

            return Mapper.Map<List<TimeSlotOutputModel>>(timeSlots);
        }

        public async Task<TimeSlotOutputModel> GetAsync(int id)
        {
            var timeSlot = await _timeSlotsRepository.GetAsync(id);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }

        public async Task<TimeSlotOutputModel> AddAsync(TimeSlotInputModel timeSlotInput)
        {
            var timeSlot = Mapper.Map<TimeSlot>(timeSlotInput);

            timeSlot = await _timeSlotsRepository.AddAsync(timeSlot);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }

        public async Task<TimeSlotOutputModel> UpdateAsync(int id, TimeSlotInputModel timeSlotInput)
        {
            var timeSlot = Mapper.Map<TimeSlot>(timeSlotInput);
            timeSlot.Id = id;

            timeSlot = await _timeSlotsRepository.UpdateAsync(timeSlot);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _timeSlotsRepository.DeleteAsync(id);
        }

        public async Task<TicketOutputModel> GetTicketsAsync(int timeSlotId)
        {
            throw new NotImplementedException();
        }
    }
}
