using AutoMapper;

using Seats4Me.API.Models;

namespace Seats4Me.API.Tests
{
    public abstract class MapperTest
    {
        private static bool _isInitialized;
        private static readonly object Lock = new object();

        protected MapperTest()
        {
            lock (Lock)
            {
                if (_isInitialized)
                {
                    return;
                }

                _isInitialized = true;
                Mapper.Initialize(cfg => { cfg.AddProfile(new Seats4MeProfile()); });
            }
        }
    }
}