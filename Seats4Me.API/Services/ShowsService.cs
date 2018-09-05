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
    public class ShowsService : IShowsService
    {
        private readonly IShowsRepository _showsRepository;

        public ShowsService(IShowsRepository showsRepository)
        {
            _showsRepository = showsRepository;
        }

        public async Task<List<ShowOutputModel>> GetAsync()
        {
            var shows = await _showsRepository.GetAsync();

            return Mapper.Map<List<ShowOutputModel>>(shows);
        }

        public async Task<ShowOutputModel> GetAsync(int id)
        {
            var show = await _showsRepository.GetAsync(id);

            return Mapper.Map<ShowOutputModel>(show);
        }

        public async Task<ShowOutputModel> CreateAsync(ShowInputModel showInput)
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
    }
}
