using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Dtos
{
    public class TouristRouteUpdateDto: TouristRouteManipulationDto
    {
        [Required(ErrorMessage = "Description is required when updating.")]
        [MaxLength(1500)]
        public override string Description { get; set; }
      
    }
}
