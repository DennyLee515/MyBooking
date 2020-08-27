using MyBooking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Services
{
    public interface ITouristRouteRepository
    {
        IEnumerable<TouristRoute> GetTouristRoutes();
        TouristRoute GetTourist(Guid touristRouteId);
    }
}
