using Mango.Service.AuthAPI.Model.Dto;
using Mango.Service.AuthAPI.Models.Dto;
using Mango.Service.AuthAPI.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Service.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService authService;
        protected ResponseDto responseDto;
        public AuthAPIController(IAuthService authService)
        {
            this.authService = authService;
            responseDto = new();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var errorMessage = await authService.Register(model);

            if(!string.IsNullOrEmpty(errorMessage))
            {
                responseDto.IsSucess = false;
                responseDto.Message = errorMessage;
                return BadRequest(responseDto);
            }
            return Ok(responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var loginResponse = await authService.Login(loginRequestDto);
            if(loginResponse.User == null)
            {
                responseDto.IsSucess = false;
                responseDto.Message = "Username and password is incorrect";

                return BadRequest(responseDto);
            }

            responseDto.Result = loginResponse;

            return Ok(responseDto);
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto registrationRequestDto)
        {
            var assignRoleSuccessfully = await authService.AssignRole(registrationRequestDto.Email,registrationRequestDto.Role.ToUpper());
            if (!assignRoleSuccessfully)
            {
                responseDto.IsSucess = false;
                responseDto.Message = "Error encountered";

                return BadRequest(responseDto);
            }

            
            return Ok(responseDto);
        }
    }
}
