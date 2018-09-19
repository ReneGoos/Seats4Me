using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Services
{
    public class TimeSlotsService : ITimeSlotsService
    {
        private readonly ITimeSlotsRepository _timeSlotsRepository;

        public TimeSlotsService(ITimeSlotsRepository timeSlotsRepository)
        {
            _timeSlotsRepository = timeSlotsRepository;
        }

        public async Task<TimeSlotOutputModel> AddAsync(int showId, TimeSlotInputModel timeSlotInput)
        {
            var timeSlot = Mapper.Map<TimeSlot>(timeSlotInput);

            timeSlot.ShowId = showId;
            timeSlot = await _timeSlotsRepository.AddAsync(timeSlot);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }

        public async Task DeleteAsync(int showId, int id)
        {
            var timeSlot = await _timeSlotsRepository.GetAsync(showId, id);

            if (timeSlot == null)
            {
                return;
            }

            await _timeSlotsRepository.DeleteAsync(timeSlot);
        }

        public async Task<bool> ExistsAsync(int showId, int id)
        {
            return await _timeSlotsRepository.GetAsync(id) != null;
        }

        public async Task<List<TimeSlotOutputModel>> GetAsync(int showId)
        {
            var timeSlots = await _timeSlotsRepository.GetAsync(showId);

            return Mapper.Map<List<TimeSlotOutputModel>>(timeSlots);
        }

        public async Task<TimeSlotOutputModel> GetAsync(int showId, int id)
        {
            var timeSlot = await _timeSlotsRepository.GetAsync(showId, id);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }

        public async Task<decimal> GetPriceAsync(int showId, int id, bool discount)
        {
            var timeSlot = await _timeSlotsRepository.GetAsync(showId, id);

            return timeSlot.PromoPrice > 0 ? timeSlot.PromoPrice : timeSlot.Show.RegularDiscountPrice > 0 && discount ? timeSlot.Show.RegularDiscountPrice : timeSlot.Show.RegularPrice;
        }

        public async Task<TimeSlotOutputModel> UpdateAsync(int showId, int id, TimeSlotInputModel timeSlotInput)
        {
            var timeSlot = Mapper.Map<TimeSlot>(timeSlotInput);

            timeSlot.Id = id;
            timeSlot.ShowId = showId;

            timeSlot = await _timeSlotsRepository.UpdateAsync(timeSlot);

            return Mapper.Map<TimeSlotOutputModel>(timeSlot);
        }
    }
}