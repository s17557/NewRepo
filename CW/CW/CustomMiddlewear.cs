using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW
{
    public class CustomMiddlewear
    {
        private RequestDelegate _next;
        public CustomMiddlewear(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync (HttpContext context)
        {   //jeśli jakiś żądanie w nagłówku nie posiada konkretnego klucza może zwrócić kod 401
            context.Response.Headers.Add("index", "s17557"); 


            await _next.Invoke(context);
        }

    }
}
