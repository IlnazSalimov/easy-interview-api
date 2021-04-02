using System.IO;
using System.Text;
using EasyInterview.API.BusinessLogic.Services.Interview;
using EasyInterview.API.BusinessLogic.Services.User;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.Data;
using EasyInterview.API.DataAccess.Entities;
using EasyInterview.API.DataAccess.Repositories;
using EasyInterview.API.DataAccess.Repositories.Interview;
using EasyInterview.API.DataAccess.Repositories.User;
using EasyInterview.API.Infrastructure.Middlewares.ExceptionHandler;
using EasyInterview.API.Settings.AppSettings;
using EasyInterview.API.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace EasyInterview.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ILogger Logger { get; set; }
        public IConfiguration Configuration { get; }
        public const string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder.WithOrigins("http://localhost:4201", "https://localhost:4201", "https://192.168.0.15:4201")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services
                .AddSignalR();

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            CreateIdentityIfNotCreated(services);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Authentication:Jwt:Secret"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            })
              .AddGoogle(options =>
              {
                  options.ClientId = Configuration["Authentication:Google:ClientId"];
                  options.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
              });

            services.AddAuthorization();

            services.AddTransient<IRepository<IEntity>, Repository<IEntity>>();
            services.AddTransient<IInterviewRepository, InterviewRepository>();
            services.AddTransient<IInterviewService, InterviewService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddDbContext<EasyInterviewContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("EasyInterviewContext")));
            services.Configure<AuthSettings>(Configuration.GetSection("Authentication"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCustomExceptionHandler();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRtcHub>("/signalrtc");
            });
            app.UseHsts();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private static void CreateIdentityIfNotCreated(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var existingUserManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
            if (existingUserManager == null)
            {
                services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<EasyInterviewContext>()
                        .AddDefaultTokenProviders();
            }
        }
    }
}
