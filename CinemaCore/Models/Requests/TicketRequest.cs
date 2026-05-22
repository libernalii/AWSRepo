using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaCore.Models.Requests
{
    public class TicketRequest
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; }
    }
}
