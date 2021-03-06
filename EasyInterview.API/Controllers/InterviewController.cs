﻿using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.BusinessLogic.Services.Interview;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.DataAccess.Repositories.Interview;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EasyInterview.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InterviewController : ControllerBase
    {
        protected readonly UserManager<AppUser> UserManager;
        public IInterviewService InterviewService { get; set; }

        public InterviewController(IInterviewService interviewService, UserManager<AppUser> userManager)
        {
            InterviewService = interviewService;
            UserManager = userManager;
        }

        [HttpGet("{id}", Name = nameof(GetInterview))]
        public async Task<IActionResult> GetInterview(int id)
        {
            InterviewModel interview = await InterviewService.Get(id);

            if (interview == null)
            {
                return BadRequest();
            }

            return Ok(interview);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateInteviewModel model)
        {
            var user = await UserManager.FindByIdAsync(User.Identity.Name);
            var id = await InterviewService.Create(model, user);
            return CreatedAtRoute(nameof(GetInterview), new { id }, await InterviewService.Get(id));
        }
    }
}
