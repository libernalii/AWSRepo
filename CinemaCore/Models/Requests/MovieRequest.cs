using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaCore.Models.Requests
{
    public class MovieRequest
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationMinutes { get; set; }
        public string Description { get; set; }
        public DateTime ShowTime { get; set; }
    }
}
