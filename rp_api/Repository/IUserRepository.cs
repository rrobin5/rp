using rp_api.Model;

namespace rp_api.Repository
{
    public interface IUserRepository
    {
        Task<User> FindUserByUsernameAsync(string username);
        Task CreateUserAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
    }
}