using CinemaCore.Models;
using CinemaCore.Models.Requests;

namespace CinemaCore.Interfaces
{
    public interface IMovieService
    {
        List<Movie> GetAll();
        Movie Create(MovieRequest movie);
        Movie Update(int id, MovieRequest movie);
        void Delete(int id);
    }
}
