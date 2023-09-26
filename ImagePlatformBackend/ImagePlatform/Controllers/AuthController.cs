using ImagePlatform.Models;
using ImagePlatform.Models.Dto;
using ImagePlatform.Repository.UserRepository;
using ImagePlatform.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ImagePlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public AuthController(IConfiguration configuration, IUserService userService,
            IUserRepository userRepository)
        {
            this.configuration = configuration;
            this.userService = userService;
            this.userRepository = userRepository;
        }

        [HttpPost("user/{id}")]
        public ActionResult<User> GetUserById(Guid id)
        {
            var user = userRepository.GetUser(id);
            return Ok(user);
        }

        [HttpPost("{token}")]
        public ActionResult<User> GetUserByToken(string token)
        {
            var user = userRepository.getUserByToken(token);
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserRegisterDto request)
        {

            if (request.Password == request.ConfirmPassword)
            {
                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var newUser = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                userRepository.AddUser(newUser);

                return Ok(newUser);
            }

            return BadRequest("Passwords must match!");
        }


        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLoginDto request)
        {

            if (!userRepository.UserExists(request.Username))
            {
                return BadRequest("User not found!");
            }

            var user = userRepository.getUserByUsername(request.Username);
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password!");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, user);
            user.AuthToken = token;
            userRepository.UpdateUser(user);
          
            return Ok(token);
        }

        //[HttpPost("refresh-token")]
        //public async Task<ActionResult<string>> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];

        //    if (!user.RefreshToken.Equals(refreshToken))
        //    {
        //        return Unauthorized("Invalid refresh token!");
        //    }
        //    else if (user.TokenExpires < DateTime.Now)
        //    {
        //        return Unauthorized("Token Expired.");
        //    }

        //    string token = CreateToken(user);
        //    var newRefreshToken = GenerateRefreshToken();
        //    SetRefreshToken(newRefreshToken, user);

        //    return Ok(token);
        //}

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private void SetRefreshToken(RefreshToken newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
