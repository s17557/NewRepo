using CW.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW.Middlewares
{
    public class ExceptionMiddlewares
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddlewares(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception x)
            {
                await HandleExceptionAsync(context, x);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exc)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = "Wystąpił błąd: " + exc
            }.ToString());
        }
    }
}
