using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.Controllers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.BusinessLogic.Services.Interview
{
    public interface IInterviewService
    {
        Task<int> Create(CreateInteviewModel model);
        Task<InterviewModel> Get(int id);
    }
}
