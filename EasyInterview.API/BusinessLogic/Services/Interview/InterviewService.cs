using AutoMapper;
using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.DataAccess.Entities;
using EasyInterview.API.DataAccess.Repositories.Interview;
using EasyInterview.API.DataAccess.Repositories.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.BusinessLogic.Services.Interview
{
    public class InterviewService : IInterviewService
    {
        public IInterviewRepository InterviewRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IMapper Mapper { get; set; }

        public InterviewService(
            IInterviewRepository interviewRepository, 
            IUserRepository userRepository, 
            IMapper mapper)
        {
            InterviewRepository = interviewRepository;
            UserRepository = userRepository;
            Mapper = mapper;
        }

        public async Task<int> Create(CreateInteviewModel model)
        {
            var owner = UserRepository.GetAll().First(u => u.Email == model.OwnerEmail);

            var interview = Mapper.Map<InterviewEntity>(model);
            interview.CreatedDate = DateTime.Now;
            interview.Owner = owner;

            return await InterviewRepository.Save(interview);
        }

        public async Task<InterviewModel> Get(int id)
        {
            var interview = await InterviewRepository.Get(id);
            return Mapper.Map<InterviewModel>(interview);
        }
    }
}
