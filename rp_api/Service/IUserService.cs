using rp_api.DTO;

namespace rp_api.Service
{
    public interface IUserService
    {
        Task CreateUser(UserRequest userRequest);
        Task<LoginResponse> Login(LoginRequest loginRequest);
        Task<List<string>> GetAllUsernames();
    }
}