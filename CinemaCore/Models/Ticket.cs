using System;
using System.Collections.Generic;
using System.Text;

namespace CinemaCore.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        // Зв’язок з користувачем
        public int UserId { get; set; }
        public User User { get; set; }

        // Зв’язок з фільмом
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string Status { get; set; } // Active / Cancelled
    }
}
