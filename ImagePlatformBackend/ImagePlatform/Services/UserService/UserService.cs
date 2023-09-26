using ImagePlatform.Models;
using ImagePlatform.Repository.UserRepository;
using System.Security.Claims;

namespace ImagePlatform.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository userRepository;

        public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
        }

        public User addUser(User user)
        {
            userRepository.AddUser(user);
            return user;
        }

        public string GetUsername()
        {
            var result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return result;
        }
    }
}
