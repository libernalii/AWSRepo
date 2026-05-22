using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CinemaCore.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
