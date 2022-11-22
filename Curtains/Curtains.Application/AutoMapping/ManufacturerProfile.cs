﻿using AutoMapper;
using Curtains.Application.DTO;
using Curtains.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curtains.Application.AutoMapping
{
    public class ManufacturerProfile : Profile
    {
        public ManufacturerProfile()
        {
            CreateMap<ManufacturerModel, ManufacturerDTO>().ReverseMap();
        }
    }
}
