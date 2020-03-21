using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LessPaper.APIGateway.Helper;
using LessPaper.APIGateway.Interfaces.External.GuardApi;
using LessPaper.APIGateway.Interfaces.External.ReadApi;
using LessPaper.APIGateway.Interfaces.External.WriteApi;
using LessPaper.APIGateway.Models;
using LessPaper.APIGateway.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LessPaper.APIGateway.Controllers.v1
{
    [Route("v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<AppSettings> config;
        private readonly IGuardApi guardApi;

        public UserController(IOptions<AppSettings> config, IGuardApi guardApi)
        {
            this.config = config;
            this.guardApi = guardApi;
        }


        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest registrationRequest) 
        {
            // Validate user input
            if (string.IsNullOrEmpty(registrationRequest.Password) || 
                string.IsNullOrEmpty(registrationRequest.Email) ||
                registrationRequest.Password.Length < config.Value.ValidationRules.MinimumPasswordLength ||
                !ValidationHelper.IsValidEmailAddress(registrationRequest.Email))
            {
                return BadRequest();
            }

            // Generate user entry
            var emailAddress = registrationRequest.Email;
            var salt = CryptoHelper.GetSalt(10);
            var hashedPassword = CryptoHelper.Sha256FromString(registrationRequest.Password, salt);
            var userId = CryptoHelper.GetGuid();
            
            // Call api to register a new user
            var registrationSuccessful = await guardApi.RegisterUser(emailAddress,  hashedPassword, salt, userId);
            if (!registrationSuccessful)
                return BadRequest();
            
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest loginRequest)
        {
            // Validate user input
            if (string.IsNullOrEmpty(loginRequest.Password) ||
                string.IsNullOrEmpty(loginRequest.Email) ||
                loginRequest.Password.Length < config.Value.ValidationRules.MinimumPasswordLength ||
                !ValidationHelper.IsValidEmailAddress(loginRequest.Email))
            {
                return BadRequest();
            }
            
            // Receive user data
            var userData = await guardApi.GetUserLoginInformation(loginRequest.Email);
            if (userData == null)
                return BadRequest();

            // Recalculate the password and compare with given password hash
            var hashedPassword = CryptoHelper.Sha256FromString(loginRequest.Password, userData.Salt);
            if (hashedPassword != userData.PasswordHash)
                return BadRequest();
            
            // Todo generate token

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] AuthToken oldAuthToken)
        {
            var newAuthToken = new AuthToken();

            // Todo implement token refresh
            await Task.Delay(10);
            
            return Ok(newAuthToken);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UserInfo()
        {
            //Todo implement request for user information
            await Task.Delay(10);
            return Ok();
        }
    }
}
