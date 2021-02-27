using EasyInterview.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.DataAccess.Repositories.User
{
    public interface IUserRepository : IRepository<OidcUserEntity>
    {
    }
}
