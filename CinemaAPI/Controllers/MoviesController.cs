using CinemaCore.Interfaces;
using CinemaCore.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using CinemaStorage.Services;


namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        private readonly S3Service _s3Service;

        public MoviesController(IMovieService service, S3Service s3Service)
        {
            _service = service;
            _s3Service = s3Service;
        }

        public MoviesController(IMovieService service)
        {
            _service = service;

        }

        // ВСІ БАЧАТЬ ФІЛЬМИ
        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _service.GetAll();
            return Ok(movies);
        }

        // ТІЛЬКИ ADMIN
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(MovieRequest movie)
        {
            var result = _service.Create(movie);
            return Ok(result);
        }

        // ТІЛЬКИ ADMIN
        //[Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, MovieRequest movie)
        {
            var result = _service.Update(id, movie);
            return Ok(result);
        }

        // ТІЛЬКИ ADMIN
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok("Deleted");
        }

        [HttpPost("upload-poster")]
        public async Task<IActionResult> UploadPoster(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not selected");

            var url = await _s3Service.UploadFileAsync(file);

            return Ok(new
            {
                PosterUrl = url
            });
        }

    }
}
