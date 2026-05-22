using CinemaCore.Models;
using CinemaStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("dynamo")]
    public class DynamoController : ControllerBase
    {
        private readonly DynamoDbService _service;

        public DynamoController(DynamoDbService service)
        {
            _service = service;
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveUser(DynamoUser user)
        {
            await _service.SaveUserAsync(user);

            return Ok(new
            {
                Message = "User saved to DynamoDB"
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _service.GetUserAsync(id);

            return Ok(user);
        }
    }
}