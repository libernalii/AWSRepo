using CinemaCore.Models;

namespace CinemaCore.Interfaces
{
    public interface ITicketService
    {
        Ticket Buy(int userId, int movieId);
        List<Ticket> GetUserTickets(int userId);
        List<Ticket> GetAll();
        Ticket Cancel(int id);
    }
}
