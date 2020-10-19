using MyBooking.API.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.ValidationAttributes
{
    public class TouristTitleDescriptionDifferent: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteManipulationDto)validationContext.ObjectInstance;
            if(touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                    "Route tilte and description must be different.",
                    new[] { "TouristRouteCreationDto" }
                    );
            }
            return ValidationResult.Success;
        }
    }
}
