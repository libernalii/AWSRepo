using CinemaAPI.DTOs;
using CinemaCore.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("tickets")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _service;

        public TicketsController(ITicketService service)
        {
            _service = service;
        }

        // Купити квиток
        //[Authorize]
        [HttpPost]
        public IActionResult Buy(CreateTicketRequest request)
        {
            var ticket = _service.Buy(request.UserId, request.MovieId);

            return Ok(ticket);
        }

        // Мої квитки
        //[Authorize]
        [HttpGet("user/{id}")]
        public IActionResult GetMy(int id)
        {
            var tickets = _service.GetUserTickets(id);

            return Ok(tickets);
        }

        // Всі квитки
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var tickets = _service.GetAll();
            return Ok(tickets);
        }

        // Скасування
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Cancel(int id)
        {
            var result = _service.Cancel(id);

            return Ok(result);
        }
    }
}
