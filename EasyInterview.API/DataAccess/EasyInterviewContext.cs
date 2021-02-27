using EasyInterview.API.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Data
{
    public class EasyInterviewContext : DbContext
    {
        public EasyInterviewContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        public DbSet<OidcUserEntity> Users { get; set; }
        public DbSet<InterviewEntity> Interviews { get; set; }
    }
}
