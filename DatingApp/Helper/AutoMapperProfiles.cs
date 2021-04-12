﻿using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entity;
using DatingApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Helper
{
    public class AutoMapperProfiles :Profile //maps one object to another
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>() 
                .ForMember(dest=>dest.PhotoUrl,opt=>opt.MapFrom(src=>  //give it an destination property ii spunem de unde vrem sa mapam si sursa de unde mapam
                src.Photos.FirstOrDefault(x=>x.IsMain).Url))
                .ForMember(dest=>dest.Age,opt=>opt.MapFrom(src=>
                src.DateOfBirth.CalculateAge())); //mapam o proprietate
            CreateMap<Photo, PhotoDto>();

        }
    }
}
