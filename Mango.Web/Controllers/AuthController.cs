using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utillity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            ResponseDto responseDto = await _authService.Login(loginRequestDto);

            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                _tokenProvider.SetToken(loginResponseDto.Token);

                await SignInAsync(loginResponseDto);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", responseDto.Message);
                return View();
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.RoleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text = SD.RoleCustomer , Value = SD.RoleCustomer },
                new SelectListItem{ Text = SD.RoleAdmin , Value = SD.RoleAdmin }
            };
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto response = await _authService.Register(obj);
            ResponseDto roles = new();

            if (response.IsSuccess && response != null)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = SD.RoleCustomer;
                }

                roles = await _authService.AssignRoles(obj);

                if (roles.IsSuccess && roles != null)
                {
                    TempData["success"] = "Registration successfully";
                    return RedirectToAction(nameof(Login));
                }
            }

            ViewBag.RoleList = new List<SelectListItem>()
            {
                new SelectListItem{ Text = SD.RoleCustomer , Value = SD.RoleCustomer },
                new SelectListItem{ Text = SD.RoleAdmin , Value = SD.RoleAdmin }
            };

            TempData["error"] = response.Message;
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }
        private async Task SignInAsync(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
