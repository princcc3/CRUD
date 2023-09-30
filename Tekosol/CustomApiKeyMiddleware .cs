using Microsoft.AspNetCore.DataProtection.KeyManagement;

public class CustomApiKeyMiddleware
{
    private readonly IConfiguration Configuration;
    private readonly RequestDelegate _next;
    const string API_KEY = "Api_Key";
    public CustomApiKeyMiddleware(RequestDelegate next,
    IConfiguration configuration)
    {
        _next = next;
        Configuration = configuration;
    }
    public async Task Invoke(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        Console.WriteLine(token);
        if (token == null)
        {
            httpContext.Response.StatusCode = 401;
            await httpContext.Response.WriteAsync("The Api Key for accessing this endpoint is not available");
            return;
        }
        else
        {
            if (token == "thisisthetoken")
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Response.StatusCode = 401;
                await httpContext.Response.WriteAsync("The authentication key is incorrect : Unauthorized access");
                return;
            }
        }
        await _next(httpContext);
    }
}