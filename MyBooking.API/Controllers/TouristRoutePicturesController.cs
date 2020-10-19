using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBooking.API.Dtos;
using MyBooking.API.Models;
using MyBooking.API.Services;

namespace MyBooking.API.Controllers
{
    [Route("api/touristRoutes/{touristRouteId}/pictures")]
    [ApiController]
    public class TouristRoutePicturesController : ControllerBase
    {
        private ITouristRouteRepository _touristRouteRepository;
        private IMapper _mapper;

        public TouristRoutePicturesController(ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _touristRouteRepository = touristRouteRepository ?? throw new ArgumentNullException(nameof(touristRouteRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public IActionResult GetPicListForTouristRoute(Guid touristRouteId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("No such route found");
            }

            var picturesFromRepo = _touristRouteRepository.GetPicsByTouristRouteId(touristRouteId);
            if (picturesFromRepo == null || picturesFromRepo.Count() <= 0)
            {
                return NotFound("No picture found");
            }

            return Ok(_mapper.Map<IEnumerable<TouristRoutePicDto>>(picturesFromRepo));
        }

        [HttpGet("{pictureId}", Name = "GetPicture")]
        public IActionResult GetPicture(Guid touistRouteId, int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touistRouteId))
            {
                return NotFound("No such route found");
            }

            var pictureFromRepo = _touristRouteRepository.GetPic(pictureId);
            if(pictureFromRepo == null)
            {
                return NotFound("No picture found");
            }

            return Ok(_mapper.Map<TouristRoutePicDto>(pictureFromRepo));
        }

        [HttpPost]
        public IActionResult CreateTouristRoutePicture(
            [FromRoute] Guid touristRouteId,
            [FromBody] TouristRoutePicCreationDto touristRoutePicCreationDto
            )
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("No such route found");
            }

            var pictureModel = _mapper.Map<TouristRoutePic>(touristRoutePicCreationDto);
            _touristRouteRepository.AddTouristRoutePic(touristRouteId, pictureModel);
            _touristRouteRepository.Save();

            var pictureToReturn = _mapper.Map<TouristRoutePicDto>(pictureModel);
            return CreatedAtRoute(
                "GetPicture",
                new
                {
                    touristRouteId = pictureModel.TouristRouteId,
                    pictureId = pictureModel.Id
                },
                pictureToReturn
                );
        }

        [HttpDelete("{pictureId}")]
        public IActionResult DeletePicture([FromRoute] Guid touristRouteId, [FromRoute] int pictureId)
        {
            if (!_touristRouteRepository.TouristRouteExists(touristRouteId))
            {
                return NotFound("No such route found");
            }

            var picture = _touristRouteRepository.GetPic(pictureId);
            _touristRouteRepository.DeleteTouristRoutePicture(picture);
            _touristRouteRepository.Save();

            return NoContent();
        }
    }
}
