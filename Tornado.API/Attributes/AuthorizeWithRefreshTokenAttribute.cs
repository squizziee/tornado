using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tornado.Infrastructure.Services.Interfaces;

namespace Tornado.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthorizeWithRefreshTokenAttribute : TypeFilterAttribute
    {

        //public AuthorizeWithRefreshTokenAttribute(IJwtService jwtService) 
        //{
        //    _jwtService = jwtService;
        //}

        //public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        //{
        //    var headers = context.HttpContext.Request.Headers;

        //    var refreshHeader = headers["Tornado-Refresh-Token"].FirstOrDefault();

        //    if (refreshHeader == null)
        //    {
        //        context.HttpContext.Response.StatusCode = 401;
        //        await context.HttpContext.Response.WriteAsync("Refresh token expired, new login is required to access the resource");
        //        return;
        //    }

        //    // Proceed to the next action in the pipeline
        //    await next();
        //}
        public AuthorizeWithRefreshTokenAttribute() : base(typeof(AuthorizeWithRefreshTokenActionFilter))
        {
        }
    }

    class AuthorizeWithRefreshTokenActionFilter : IAsyncActionFilter
    {
        private readonly IJwtService _jwtService;

        public AuthorizeWithRefreshTokenActionFilter(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Cookies.TryGetValue("refreshToken", out var token))
            {
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.Headers.ContentType = "text/html";
                await context.HttpContext.Response.WriteAsync("Refresh token not found, login is required to access the resource");
                return;
            }

            if (!_jwtService.VerifyRefreshToken(token))
            {
                await context.HttpContext.Response.WriteAsync("Refresh token is expired/corrupted, new login is required to access the resource");
                return;
            }

            await next();
        }
    }
}
