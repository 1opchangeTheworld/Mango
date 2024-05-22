using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utillity;

namespace Mango.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRoles(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.AuthAPIBase + "/api/auth/assignRole",
                Data = registrationRequestDto,
            });
        }

        public async Task<ResponseDto?> Login(LoginRequestDto requestDto)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.AuthAPIBase + "/api/auth/login",
                Data = requestDto,
            });
        }

        public async Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Url = SD.AuthAPIBase + "/api/auth/register",
                Data = registrationRequestDto,
            });
        }
    }
}
