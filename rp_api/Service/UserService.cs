using AutoMapper;
using rp_api.DTO;
using rp_api.Helper;
using rp_api.Model;
using rp_api.Repository;
using rp_api.Token;
using System.Security.Cryptography;
using System.Text;

namespace rp_api.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IHelper _helper;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IMapper mapper, IHelper helper, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _helper = helper;
            _tokenService = tokenService;
        }

        public async Task CreateUser(UserRequest userRequest)
        {
            if (await _userRepository.UsernameExistsAsync(userRequest.Username))
                throw new InvalidOperationException("Username already exists.");

            User user = _mapper.Map<User>(userRequest);

            user.Password = _helper.HashPassword(user.Password);

            await _userRepository.CreateUserAsync(user);
        }

        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            User user = await _userRepository.FindUserByUsernameAsync(loginRequest.Username);
            if (user == null || !VerifyPassword(loginRequest.Password, user.Password))
            {
                return null;
            }

            string token = _tokenService.GenerateJwtToken(user);
            LoginResponse loginResponse = new LoginResponse
            {
                Token = token,
                UserId = user.Id.ToString(),
            };

            return loginResponse;
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            using var sha256 = SHA256.Create();
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(enteredPassword));
            var enteredPasswordHash = Convert.ToBase64String(hash);
            return storedPasswordHash == enteredPasswordHash;
        }
    }
}
