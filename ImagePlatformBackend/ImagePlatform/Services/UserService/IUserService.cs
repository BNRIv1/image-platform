using ImagePlatform.Models;

namespace ImagePlatform.Services.UserService
{
    public interface IUserService
    {
        string GetUsername();
        User addUser(User user);
    }
}
