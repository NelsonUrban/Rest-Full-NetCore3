﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Helpers;

namespace CourseLibrary.API.Profiles
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, Models.AuthorDto>()
                  .ForMember(
                 dest => dest.Name,
                 opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                  .ForMember(
                dest => dest.Age,
                opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

              CreateMap<Models.AuthorForCreationDto, Entities.Author>();

        }

    }
}
