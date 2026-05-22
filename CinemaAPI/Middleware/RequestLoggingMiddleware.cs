namespace CinemaAPI.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var time = DateTime.Now;

            var user = context.User?.Identity?.Name ?? "Anonymous";
            Console.WriteLine($"[{time}] {method} {path} | User: {user}");

            await _next(context);
        }
    }
}
