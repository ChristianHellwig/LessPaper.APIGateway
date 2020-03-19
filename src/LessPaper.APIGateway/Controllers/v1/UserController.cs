using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessPaper.APIGateway.Helper;
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
    [Route("v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IOptions<AppSettings> config;
        private readonly IReadApi readApi;
        private readonly IWriteApi writeApi;

        public UserController(IOptions<AppSettings> config, IReadApi readApi, IWriteApi writeApi)
        {
            this.config = config;
            this.readApi = readApi;
            this.writeApi = writeApi;
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
            
            // Call write api to add user
            var result = await writeApi.AddUser(emailAddress,  hashedPassword, salt, userId);



            return Ok(result);
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

            


            await Task.Delay(10); //Add async business logic
            return Ok();
        }

        public void RefreshToken()
        {

        }

        public void UserInfo()
        {

        }
    }
}
