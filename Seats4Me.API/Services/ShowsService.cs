using AutoMapper;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Seats4Me.API.Models.Search;
using Seats4Me.Data.Common;

namespace Seats4Me.API.Services
{
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public async Task<IEnumerable<ShowOutputModel>> GetAsync(ShowSearchModel searchModel)
        {
            var shows = await GetOnPeriodAsync(searchModel);

            return shows;
        }

        public async Task<ShowOutputModel> GetAsync(int id)
        {
            var show = await _showsRepository.GetAsync(id);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public async Task<ShowOutputModel> AddAsync(ShowInputModel showInput)
        {
            var show = Mapper.Map<Show>(showInput);

            show = await _showsRepository.AddAsync(show);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public async Task<ShowOutputModel> UpdateAsync(int id, ShowInputModel showInput)
        {
            var show = Mapper.Map<Show>(showInput);
            show.Id = id;

            show = await _showsRepository.UpdateAsync(show);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _showsRepository.DeleteAsync(id);
        }
        private async Task<IEnumerable<ShowOutputModel>> GetOnPeriodAsync(ShowSearchModel searchModel)
        {
            var today = DateTime.Today;
            var start = today;
            var end = default(DateTime);

            if (searchModel.Week != 0)
            {
                if (searchModel.Year == 0) searchModel.Year = today.Year;

                start = DateTimeExtensions.FirstDayOfWeek(searchModel.Week, searchModel.Year);
                end = start.AddDays(7);
            }
            else if (searchModel.Month != 0)
            {
                if (searchModel.Year == 0) searchModel.Year = today.Year;

                start = new DateTime(searchModel.Year, searchModel.Month, 1);
                end = new DateTime(searchModel.Year, searchModel.Month + 1, 1);
            }
            else
            {
                if (searchModel.Year == 0)
                {
                    searchModel.Year = today.Year;
                }
                else
                {
                    if (searchModel.Year > 0 && searchModel.Year < 100)
                        searchModel.Year = searchModel.Year + 2000;

                    start = new DateTime(searchModel.Year, 1, 1);
                }

                end = new DateTime(searchModel.Year + 1, 1, 1);
            }

            var showResults = await _showsRepository.GetAsync(start, end, searchModel.Promotion);
            return Mapper.Map<List<ShowOutputModel>>(showResults);
        }
    }
}
