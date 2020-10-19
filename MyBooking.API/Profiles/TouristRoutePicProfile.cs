using AutoMapper;
using MyBooking.API.Dtos;
using MyBooking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Profiles
{
    public class TouristRoutePicProfile:Profile
    {
        public TouristRoutePicProfile()
        {
            CreateMap<TouristRoutePic, TouristRoutePicDto>();
            CreateMap<TouristRoutePicCreationDto, TouristRoutePic>();
            CreateMap<TouristRoutePic, TouristRoutePicCreationDto>();
        }
    }
}
