using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthService
    {
        Task<ResponseDto?> AssignRoles(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> Login(LoginRequestDto requestDto);
        Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto);
    }
}
