using CinemaCore.Interfaces;
using CinemaCore.Models;
using CinemaCore.Models.Requests;
using CinemaStorage.Data;

namespace CinemaStorage.Services
{
    public class MovieService : IMovieService
    {
        private readonly AppDbContext _context;

        public MovieService(AppDbContext context)
        {
            _context = context;
        }

        public List<Movie> GetAll()
        {
            return _context.Movies.ToList();
        }

        public Movie Create(MovieRequest movieR)
        {
            var movie = MapToMovie(movieR);
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return movie;
        }

        public Movie Update(int id, MovieRequest updated)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) throw new Exception("Not found");

            movie.Title = updated.Title;
            movie.Genre = updated.Genre;
            movie.DurationMinutes = updated.DurationMinutes;
            movie.Description = updated.Description;

            _context.SaveChanges();
            return movie;
        }

        public void Delete(int id)
        {
            var movie = _context.Movies.Find(id);
            if (movie == null) throw new Exception("Not found");

            _context.Movies.Remove(movie);
            _context.SaveChanges();
        }

        public Movie MapToMovie(MovieRequest movie)
        {
            return new Movie()
            {
                Title = movie.Title,
                Genre = movie.Genre,
                DurationMinutes = movie.DurationMinutes,
                Description = movie.Description,
                ShowTime = movie.ShowTime,
                Tickets = new List<Ticket>()
            };
        }
    }
}
