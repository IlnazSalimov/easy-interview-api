using AutoMapper;
using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Settings.AutoMapping
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<InterviewEntity, InterviewModel>();
            CreateMap<OidcUserEntity, OidcUserModel>();
            CreateMap<CreateInteviewModel, InterviewEntity>();
        }
    }
}
