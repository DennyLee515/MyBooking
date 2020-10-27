using AutoMapper;
using MyBooking.API.Dtos;
using MyBooking.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBooking.API.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderDto>()
                .ForMember(
                dest => dest.State,
                opt =>
                {
                    opt.MapFrom(src => src.State.ToString());
                });
        }

    }
}
