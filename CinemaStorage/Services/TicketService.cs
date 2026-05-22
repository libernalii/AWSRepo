using CinemaCore.Interfaces;
using CinemaCore.Models;
using CinemaCore.Models.Requests;
using CinemaStorage.Data;

namespace CinemaStorage.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public Ticket Buy(int userId, int movieId)
        {
            var movie = _context.Movies.Find(movieId);
            if (movie == null) throw new Exception("Movie not found");

            var ticket = new Ticket
            {
                UserId = userId,
                MovieId = movieId,
                PurchaseDate = DateTime.Now,
                Status = "Active"
            };

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            var user = _context.Users.Find(userId);
            ticket.User = user;

            return ticket;
        }

        public List<Ticket> GetUserTickets(int userId)
        {
            return _context.Tickets
                .Where(t => t.UserId == userId)
                .ToList();
        }

        public List<Ticket> GetAll()
        {
            return _context.Tickets.ToList();
        }

        public Ticket Cancel(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null) throw new Exception("Not found");

            ticket.Status = "Cancelled";
            _context.SaveChanges();

            return ticket;
        }

        public Ticket MapToTicket(TicketRequest ticket)
        {
            return new Ticket()
            {
                UserId = ticket.UserId,
                MovieId = ticket.MovieId,
                PurchaseDate = ticket.PurchaseDate,
                Status = ticket.Status,
                User = new User(),
                Movie = new Movie(),
            };
        }
    }
}
