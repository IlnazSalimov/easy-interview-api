using EasyInterview.API.BusinessLogic.Services.Interview;
using EasyInterview.API.BusinessLogic.Services.User;
using EasyInterview.API.Data;
using EasyInterview.API.DataAccess.Entities;
using EasyInterview.API.DataAccess.Repositories;
using EasyInterview.API.DataAccess.Repositories.Interview;
using EasyInterview.API.DataAccess.Repositories.User;
using EasyInterview.API.SignalR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddDebug();
            });

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder => builder.WithOrigins("http://localhost:4200", "https://localhost:4200", "https://192.168.0.14:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddSignalR();

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            services.AddAuthentication()
                .AddGoogle(option =>
                {
                    option.ClientId = "640479877851-3voqs5v113u3tjmi0p1b9la9dm0po8kf.apps.googleusercontent.com";
                    option.ClientSecret = "EukELh_Ltt1OHY5OZTsMucyK";
                });
            //.AddJwtBearer(options =>
            //{
            //    options.Events = new JwtBearerEvents
            //    {
            //        OnAuthenticationFailed = c =>
            //        {
            //            Logger.LogError(1, c.Exception, c.Exception.Message);
            //            return LogResponse(c.HttpContext);
            //        },
            //        OnForbidden = c =>
            //        {
            //            return LogResponse(c.HttpContext);
            //        },
            //        OnChallenge = c =>
            //        {
            //            return LogResponse(c.HttpContext);
            //        },
            //        OnMessageReceived = c =>
            //        {
            //            return LogResponse(c.HttpContext);
            //        },
            //        OnTokenValidated = c =>
            //        {
            //            return LogResponse(c.HttpContext);
            //        }
            //    };

            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;

            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,

            //        ValidAudience = "640479877851-3voqs5v113u3tjmi0p1b9la9dm0po8kf.apps.googleusercontent.com",
            //        ValidIssuer = "https://accounts.google.com",
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EukELh_Ltt1OHY5OZTsMucyK")),
            //        ClockSkew = TimeSpan.Zero
            //    };
            //});
            //.AddOpenIdConnect(options =>
            //{
            //    options.ClientId = "640479877851-3voqs5v113u3tjmi0p1b9la9dm0po8kf.apps.googleusercontent.com";
            //    options.ClientSecret = "EukELh_Ltt1OHY5OZTsMucyK";
            //    options.Authority = "https://accounts.google.com";

            //    options.ResponseType = "code";
            //    options.GetClaimsFromUserInfoEndpoint = true;
            //});

            services.AddAuthorization();

            services.AddTransient<IRepository<IEntity>, Repository<IEntity>>();
            services.AddTransient<IInterviewRepository, InterviewRepository>();
            services.AddTransient<IInterviewService, InterviewService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddDbContext<EasyInterviewContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("EasyInterviewContext")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            Logger = logger;
            Logger.LogError("!!!!!!!!!!!!!!! ssdafsd fw rtretret et fgertretret !!!!!!!!!!!!!!!!!!!!!");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRtcHub>("/signalrtc");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;
            
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            Logger.LogError($"Http Response Information:{Environment.NewLine}" +
                                   $"Schema:{context.Request.Scheme} " +
                                   $"Host: {context.Request.Host} " +
                                   $"Path: {context.Request.Path} " +
                                   $"QueryString: {context.Request.QueryString} " +
                                   $"Response Body: {text}");
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
