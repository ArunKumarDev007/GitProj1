public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
       // _next = next;
     //   _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log request
        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path}");

        // Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        // Create a new memory stream...
        using (var responseBody = new MemoryStream())
        {
            // ...and use that for the temporary response body
            context.Response.Body = responseBody;

            // Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            // Log response
            _logger.LogInformation($"Response: {context.Response.StatusCode}");

            // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
