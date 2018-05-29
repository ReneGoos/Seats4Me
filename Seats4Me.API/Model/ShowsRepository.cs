using System.Collections.Generic;
using System.Threading.Tasks;
using Seats4Me.Data.Model;

namespace Seats4Me.API.Model
{
    public class ShowsRepository
    {
        public async Task<IEnumerable<Show>> GetAsync()
        {
            return new List<Show>() { new Show() {ID = 1, Name = "Hamlet"}};
        }
    }
}
