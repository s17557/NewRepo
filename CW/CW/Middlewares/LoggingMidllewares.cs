using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW.Middlewares
{
    public class LoggingMiddlewares
    {
        private readonly RequestDelegate _next;
        public LoggingMiddlewares(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            if (httpContext.Request != null)
            {
                string path = httpContext.Request.Path;
                string method = httpContext.Request.Method;
                string queryString = httpContext.Request.QueryString.ToString();
                string bodyString = "";
                using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyString = await reader.ReadToEndAsync();
                    httpContext.Request.Body.Position = 0;
                }

                string filePath = @"requestsLog.txt";
                if (!File.Exists(filePath))
                {
                    File.Create(filePath);
                }
                if (File.Exists(filePath))
                {
                    using (var tw = new StreamWriter(filePath, true))
                    {
                        tw.WriteLine(String.Format("{0} LoggingMiddleware: Method={1}; Path={2}; Body={3}; Query={4}", DateTime.Now, method, path, bodyString, queryString)); ;
                    }
                }

            }
            //Our code
            if (_next != null) await _next(httpContext);
        }
    }
}
