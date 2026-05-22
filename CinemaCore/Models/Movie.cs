using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CinemaCore.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }
        public string Description { get; set; }
        public DateTime ShowTime { get; set; }

        public string? PosterUrl { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
