using AutoMapper;
using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.DataAccess.Entities;
using EasyInterview.API.DataAccess.Repositories.Interview;
using EasyInterview.API.DataAccess.Repositories.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.BusinessLogic.Services.Interview
{
    public class InterviewService : IInterviewService
    {
        private readonly IInterviewRepository interviewRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;

        public InterviewService(
            IInterviewRepository interviewRepository,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            this.interviewRepository = interviewRepository;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<int> Create(CreateInteviewModel model, AppUser owner)
        {
            var interview = mapper.Map<InterviewEntity>(model);
            interview.CreatedDate = DateTime.Now;
            interview.Owner = owner;

            return await interviewRepository.Save(interview);
        }

        public async Task<InterviewModel> Get(int id)
        {
            var interview = await interviewRepository.Get(id);
            return mapper.Map<InterviewModel>(interview);
        }
    }
}
