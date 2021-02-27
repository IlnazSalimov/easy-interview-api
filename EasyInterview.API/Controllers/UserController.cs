using EasyInterview.API.BusinessLogic.Models;
using EasyInterview.API.BusinessLogic.Services.User;
using EasyInterview.API.Controllers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EasyInterview.API.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService UserService { get; set; }

        public UserController(IUserService userService)
        {
            UserService = userService;
        }

        [HttpGet("{id}", Name = nameof(GetUser))]
        public async Task<IActionResult> GetUser(int id)
        {
            OidcUserModel user = await UserService.Get(id);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateOidcUserModel model)
        {
            var id = await UserService.Create(model);
            return CreatedAtRoute(nameof(GetUser), new { id });
        }
    }
}
