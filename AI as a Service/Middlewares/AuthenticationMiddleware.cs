using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AI_as_a_Service.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Read the authorization header
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            var tokenString = authorizationHeader.Substring("Bearer ".Length).Trim();

            try
            {
                // Verify and read the token (you might want to replace this with your own token validation logic)
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);

                // TODO: Perform any additional checks, e.g., check if the user still exists in the database

                // Add the user's claims to the HttpContext so they can be accessed later
                //context.User = token.ClaimsPrincipal;

                // Call the next middleware
                await _next(context);
            }
            catch (Exception ex)
            {
                // Invalid token or other errors
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
    }
}
