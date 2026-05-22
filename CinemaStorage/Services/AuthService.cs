using CinemaCore.Interfaces;
using CinemaCore.Models;
using CinemaCore.Models.Requests;
using CinemaStorage.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CinemaStorage.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public string Register(UserRequest userR)
        {
            var user = MapToUser(userR);

            if (_context.Users.Any(u => u.Email == user.Email))
                throw new Exception("User already exists");

            if (string.IsNullOrWhiteSpace(user.Email))
                throw new Exception("Email is required");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new Exception("Password is required");

            if (user.Password.Length < 6)
                throw new Exception("Password must be at least 6 characters");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _context.Users.Add(user);
            _context.SaveChanges();

            return "User registered";
        }

        public string Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Email and password are required");

            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                throw new Exception("Invalid credentials");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("UserId", user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public User MapToUser(UserRequest user)
        {
            return new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                Role = user.Role,
                Tickets = new List<Ticket>()
            };
        }
    }
}
