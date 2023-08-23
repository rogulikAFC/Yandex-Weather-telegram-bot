using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Net;
using System.Runtime.InteropServices;
using WeatherAPI.DAL;

namespace WeatherAPI.Middlewares
{
    public class UserSignInCheckerMiddleware
    {
        private readonly RequestDelegate _next;

        public UserSignInCheckerMiddleware(RequestDelegate next)
        {
            _next = next
                ?? throw new ArgumentNullException();
        }

        public async Task InvokeAsync(
            HttpContext context, IUnitOfWork unitOfWork)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);

                return;
            }

            var controllerActionDescriptior = endpoint
                .Metadata
                .GetMetadata<ControllerActionDescriptor>();

            var isRegistrationMethod = context.Request
                .Path.ToString().Equals("/api/users");

            Console.WriteLine("registration method status is " + isRegistrationMethod);

            if (isRegistrationMethod)
            {
                await _next(context);

                return;
            }

            /* if (controllerActionDescriptior == null)
            {
                
                throw new Exception("controller action descriptor is null");
            }

            var actionName = controllerActionDescriptior.ActionName;

            Console.WriteLine(actionName);

            if (actionName == "RegistrateUser")
            {
                await _next(context);

                return;
            } */

            var userIdString = context.Request.Query["user_id"].ToString();

            long userId = long.TryParse(userIdString, out userId)
                ? userId
                : -1;

            var user = userId == -1
                ? null
                : await unitOfWork.UserRepository
                .GetUserByTelegramId(userId);

            if (user == null)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                await context.Response.StartAsync();
            }
            else
            {
                await _next(context);
            }
            
        }
    }

    public static class UserSignInCheckerMiddlewareExtensions
    {
        public static IApplicationBuilder UseUserSignInCheckedMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserSignInCheckerMiddleware>();
        }
    }
}
