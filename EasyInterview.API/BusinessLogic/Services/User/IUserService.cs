using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.BusinessLogic.Services.User
{
    public interface IUserService
    {
        Task<int> Create(CreateOidcUserModel model);
        Task<OidcUserModel> Get(int id);
    }
}
