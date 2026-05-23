using CinemaCore.Interfaces;
using CinemaCore.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CinemaStorage.Services;
using Microsoft.AspNetCore.Http;

namespace CinemaAPI.Controllers
{
    [ApiController]
    [Route("movies")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly S3Service _s3Service;

        // ЗАЛИШАЄМО ТІЛЬКИ ОДИН КОНСТРУКТОР З УСІМА ЗАЛЕЖНОСТЯМИ
        public MoviesController(IMovieService service, S3Service s3Service)
        {
            _service = service;
            _s3Service = s3Service;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var movies = _service.GetAll();
            return Ok(movies);
        }

        [HttpPost]
        public IActionResult Create(MovieRequest movie)
        {
            var result = _service.Create(movie);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, MovieRequest movie)
        {
            var result = _service.Update(id, movie);
            return Ok(result);
        }

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
                return BadRequest("Файл не передано.");

            try
            {
                Console.WriteLine($"[S3] Отримано файл: {file.FileName}, розмір: {file.Length} байт");
                Console.WriteLine("[S3] Надсилання запиту до AWS S3...");

                var fileUrl = await _s3Service.UploadFileAsync(file);

                Console.WriteLine($"[S3] Успіх! Файл доступний за URL: {fileUrl}");
                return Ok(new { url = fileUrl });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[S3 CRITICAL ERROR] Тип: {ex.GetType().Name} -> Повідомлення: {ex.Message}");
                Console.WriteLine($"[S3 STACK TRACE] {ex.StackTrace}");

                return StatusCode(500, new { message = "Помилка S3", error = ex.Message });
            }
        }
    }
}