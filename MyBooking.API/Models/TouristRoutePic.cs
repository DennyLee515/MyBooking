using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Models
{
    public class TouristRoutePic
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public Guid TouristRouteId { get; set; }
        public TouristRoute TouristRoute { get; set; }
    }
}
