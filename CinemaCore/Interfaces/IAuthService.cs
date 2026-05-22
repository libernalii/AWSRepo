using CinemaCore.Models;
using CinemaCore.Models.Requests;

namespace CinemaCore.Interfaces
{
    public interface IAuthService
    {
        string Register(UserRequest user);
        string Login(string email, string password);
    }
}
