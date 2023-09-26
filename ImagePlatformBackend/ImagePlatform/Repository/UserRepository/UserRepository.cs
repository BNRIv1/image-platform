using ImagePlatform.Data;
using ImagePlatform.Models;
using ImagePlatform.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ImagePlatform.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {

        private readonly DataContext _context;
        private DbSet<User> _users;

        public UserRepository(DataContext dataContext) {
            _context = dataContext;
            _users = dataContext.Set<User>();
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            _context.SaveChanges();
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public User GetUser(Guid id)
        {
            return _users.Where(x => x.Id == id).FirstOrDefault();
        }

        public User getUserByToken(string token)
        {
            return _users.Where(x => x.AuthToken == token).FirstOrDefault();
        }

        public User getUserByUsername(string username)
        {
            return _users.Where(x => x.Username == username).FirstOrDefault();
        }

        public User UpdateUser(User user)
        {
            _users.Update(user);
            _context.SaveChanges();
            return user;
        }

        public bool UserExists(string username)
        {
            return _users.Any(u => u.Username == username);
        }
    }
}
