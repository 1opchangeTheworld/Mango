using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthAPIController : ControllerBase
{
    private readonly IAuthService _authService;
    private ResponseDto _responseDto;
    public AuthAPIController(IAuthService authService)
    {
        _authService = authService;
        _responseDto = new ResponseDto();
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequestDto regiterDto)
    {
        var errorMessage = await _authService.Register(regiterDto);

        if (!string.IsNullOrEmpty(errorMessage))
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = errorMessage;
            return BadRequest();
        }
        return Ok(_responseDto);
    }
    [HttpPost("login")]
    public async Task<IActionResult> LogIn([FromBody] LoginRequestDto model)
    {
        var user = await _authService.Login(model);
        if (user.User == null)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Username or Password Invalid";
            return BadRequest(_responseDto);
        }
        _responseDto.Result = user;
        return Ok(_responseDto);
    }
    [HttpPost("AssignRole")]
    public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
    {
        var assignRolesSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
        if (!assignRolesSuccessful)
        {
            _responseDto.IsSuccess = false;
            _responseDto.Message = "Error encountered";
            return BadRequest(_responseDto);
        }
        return Ok(_responseDto);
    }
}
