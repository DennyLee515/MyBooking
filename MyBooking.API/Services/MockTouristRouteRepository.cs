using MyBooking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Services
{
    public class MockTouristRouteRepository : ITouristRouteRepository
    {
        private List<TouristRoute> _route;
        public MockTouristRouteRepository()
        {
            if(_route==null)
            {
                InitializeTouristRoutes();
            }
        }

        private void InitializeTouristRoutes()
        {
            _route = new List<TouristRoute>
            {
                new TouristRoute
                {
                    Id = Guid.NewGuid(),
                    Title = "Melbourne",
                    Description = "Melbourne is good",
                    OriginalPrice = 1299,
                    Features ="<p>coffee</p>",
                    Fees = "<p>Excludes transport fees</p>",
                    Note ="<p>Care of danger.</p>"
                }
            };
        }
        public TouristRoute GetTourist(Guid touristRouteId)
        {
            return _route.FirstOrDefault(n => n.Id == touristRouteId);
        }

        public IEnumerable<TouristRoute> GetTouristRoutes()
        {
            return _route;
        }
    }
}
