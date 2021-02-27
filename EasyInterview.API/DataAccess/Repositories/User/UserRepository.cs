using EasyInterview.API.Data;
using EasyInterview.API.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.DataAccess.Repositories.User
{
    public class UserRepository : Repository<OidcUserEntity>, IUserRepository
    {
        public UserRepository(EasyInterviewContext context) : base(context)
        {
        }
    }
}
