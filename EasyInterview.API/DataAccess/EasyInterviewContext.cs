using EasyInterview.API.Controllers.Models;
using EasyInterview.API.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Data
{
    public class EasyInterviewContext : IdentityDbContext<AppUser>
    {
        public EasyInterviewContext([NotNull] DbContextOptions options) : base(options)
        {
        }

        //public DbSet<OidcUserEntity> Users { get; set; }
        public DbSet<InterviewEntity> Interviews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>(entity => { entity.ToTable(name: "users"); });
            builder.Entity<IdentityRole>(entity => { entity.ToTable(name: "roles"); });
            builder.Entity<IdentityUserRole<string>>(entity => { entity.ToTable("user_roles"); });
            builder.Entity<IdentityUserClaim<string>>(entity => { entity.ToTable("user_claims"); });
            builder.Entity<IdentityUserLogin<string>>(entity => { entity.ToTable("user_logins"); });
            builder.Entity<IdentityUserToken<string>>(entity => { entity.ToTable("user_tokens"); });
            builder.Entity<IdentityRoleClaim<string>>(entity => { entity.ToTable("role_claims"); });

            builder.ApplyConfiguration(new AppUserConfiguration());
        }
    }

    internal class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
