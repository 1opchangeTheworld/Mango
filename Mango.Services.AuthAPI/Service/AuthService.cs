using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtToken;
        public AuthService(AppDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtToken)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtToken = jwtToken;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {

            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();  
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == loginRequestDto.UserName);

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || !isValid) 
            {
                return new LoginResponseDto { User = null,Token = ""};
            }

            //if user was found , Generate JWT Token
            string token = _jwtToken.GenerateToken(user);

            UserDto userDto = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = loginRequestDto.UserName,
                PhoneNumber = user.PhoneNumber,
            };

            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token,
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail =registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Passsword);
                if (result.Succeeded)
                {
                    var userToReturn = _db.ApplicationUsers.First(x => x.UserName == registrationRequestDto.Email);

                    UserDto userDto = new UserDto()
                    {
                        ID = userToReturn.Id,
                        Email = userToReturn.Email,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                    };

                    return "";

                }
                return result.Errors.FirstOrDefault().Description;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
