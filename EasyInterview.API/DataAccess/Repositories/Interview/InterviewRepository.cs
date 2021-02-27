using EasyInterview.API.Data;
using EasyInterview.API.DataAccess.Entities;

namespace EasyInterview.API.DataAccess.Repositories.Interview
{
    public class InterviewRepository : Repository<InterviewEntity>, IInterviewRepository
    {
        public InterviewRepository(EasyInterviewContext context) : base(context)
        {
        }
    }
}
