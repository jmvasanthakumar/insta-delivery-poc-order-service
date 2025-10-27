using InstaDelivery.Common.Context;
using InstaDelivery.Common.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace InstaDelivery.Common.Middlewares;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;

    public RequestContextMiddleware(
        RequestDelegate next,
        ILogger<RequestContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var claims = context.User.Claims;

        var name = claims.FirstOrDefault(x => x.Type == "name")?.Value;
        var preferredUsername = claims.FirstOrDefault(x => x.Type == "preferred_username")?.Value;
        var objectId = claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;

        var rc = new RequestContext
        {
            UserId = objectId ?? string.Empty,
            UserEmail = preferredUsername.IsValidEmail() ? preferredUsername : null,
            UserName = name
        };

        RequestContext.Current = rc;

        using (_logger.BeginScope(new Dictionary<string, object?>
        {
            ["UserEmail"] = rc.UserEmail,
            ["UserId"] = rc.UserId
        }))
        {
            try
            {
                await _next(context);
            }
            finally
            {
                RequestContext.Reset();
            }
        }
    }
}
