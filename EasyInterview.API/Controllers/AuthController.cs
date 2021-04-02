using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using EasyInterview.API.Controllers.Models;
using EasyInterview.API.Settings.AppSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace EasyInterview.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthSettings> authSettings;
        private readonly UserManager<AppUser> userManager;

        public AuthController(IOptions<AuthSettings> authSettings, UserManager<AppUser> userManager)
        {
            this.authSettings = authSettings;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> GoogleAuthenticate([FromBody] GoogleUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(it => it.Errors).Select(it => it.ErrorMessage));

            return Ok(GenerateUserToken(await AuthenticateGoogleUserAsync(request)));
        }

        #region Private Methods

        private UserToken GenerateUserToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(authSettings.Value.Jwt.Secret);

            var expires = DateTime.UtcNow.AddDays(7);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id) ,
                    new Claim(JwtRegisteredClaimNames.Sub, authSettings.Value.Jwt.Subject),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.Surname, user.FirstName),
                    new Claim(ClaimTypes.GivenName, user.LastName),
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                }),

                Expires = expires,

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = authSettings.Value.Jwt.Issuer,
                Audience = authSettings.Value.Jwt.Audience
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = tokenHandler.WriteToken(securityToken);

            return new UserToken
            {
                UserId = user.Id,
                Email = user.Email,
                Token = token,
                Expires = expires
            };
        }

        private async Task<AppUser> AuthenticateGoogleUserAsync(GoogleUserRequest request)
        {
            Payload payload = await ValidateAsync(request.IdToken, new ValidationSettings
            {
                Audience = new[] { authSettings.Value.Google.ClientId }
            });

            return await GetOrCreateExternalLoginUser(GoogleUserRequest.PROVIDER, payload.Subject, payload.Email, payload.GivenName, payload.FamilyName);
        }

        private async Task<AppUser> GetOrCreateExternalLoginUser(string provider, string key, string email, string firstName, string lastName)
        {
            var user = await userManager.FindByLoginAsync(provider, key);
            if (user != null)
                return user;
            user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new AppUser
                {
                    Email = email,
                    UserName = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Id = key,
                };
                await userManager.CreateAsync(user);
            }

            var info = new UserLoginInfo(provider, key, provider.ToUpperInvariant());
            var result = await userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
                return user;
            return null;

        }

        #endregion
    }
}
