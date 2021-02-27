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

namespace EasyInterview.API.BusinessLogic.Services.User
{
    public class UserService : IUserService
    {
        public IUserRepository UserRepository { get; set; }
        public IMapper Mapper { get; set; }

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            UserRepository = userRepository;
            Mapper = mapper;
        }

        public async Task<int> Create(CreateOidcUserModel model)
        {
            var user = Mapper.Map<OidcUserEntity>(model);
            return await UserRepository.Save(user);
        }

        public async Task<OidcUserModel> Get(int id)
        {
            var user = await UserRepository.Get(id);
            return Mapper.Map<OidcUserModel>(user);
        }
    }
}
