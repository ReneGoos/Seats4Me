using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;

using Seats4Me.API.Common;
using Seats4Me.API.Models.Input;
using Seats4Me.API.Models.Output;
using Seats4Me.API.Models.Search;
using Seats4Me.API.Repositories;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Services
{
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public async Task<ShowOutputModel> AddAsync(ShowInputModel showInput)
        {
            var show = Mapper.Map<Show>(showInput);

            show = await _showsRepository.AddAsync(show);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public async Task DeleteAsync(int id)
        {
            var show = await _showsRepository.GetAsync(id);

            if (show == null)
            {
                return;
            }

            await _showsRepository.DeleteAsync(show);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _showsRepository.GetAsync(id) != null;
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

        public async Task<ShowOutputModel> UpdateAsync(int id, ShowInputModel showInput)
        {
            var show = Mapper.Map<Show>(showInput);
            show.Id = id;

            show = await _showsRepository.UpdateAsync(show);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public Task<string> GetExportAsync()
        {
            return  _showsRepository.GetExportAsync();
        }

        private async Task<IEnumerable<ShowOutputModel>> GetOnPeriodAsync(ShowSearchModel searchModel)
        {
            var start = default(DateTime);
            var end = default(DateTime);

            if (searchModel.Week != 0)
            {
                start = DateTimeExtensions.FirstDayOfWeek(searchModel.Week, searchModel.Year);
                end = start.AddDays(7);
            }
            else if (searchModel.Month != 0)
            {
                start = new DateTime(searchModel.Year, searchModel.Month, 1);
                end = new DateTime(searchModel.Year, searchModel.Month + 1, 1);
            }
            else
            {
                start = new DateTime(searchModel.Year, 1, 1);
                end = new DateTime(searchModel.Year + 1, 1, 1);
            }

            var showResults = await _showsRepository.GetAsync(start, end, searchModel.Promotion);

            return Mapper.Map<List<ShowOutputModel>>(showResults);
        }
    }
}