using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using CW.Middlewares;
using CW.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.VisualBasic;

namespace CW
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IStudentsDal, SqlServerDBDal>();
            services.AddTransient<IDBService, MockDBService>(); //wstrzykiwanie kontrolera zalerzności
            services.AddTransient<IStudentDBService, SqlServerStudentDBService>();
            services.AddSingleton<IStudentsDel, SqlServerDBDal>();
            services.AddControllers();
            
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "Student App Api", Version = "v1" });

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IStudentDBService service)
        {
            //strona z opisem błędu
            app.UseDeveloperExceptionPage();

            app.UseRouting();

            //doklejanie do nagłówka http
            app.Use(async (context, c) =>
            { //klasa http context do odpowiedzi serwera do nagłówka dodanie
                context.Response.Headers.Add("Secret", "s17557");
                await c.Invoke();

            });
            app.UseMiddleware<ExceptionMiddlewares>();
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Students App API");
            });

            app.UseMiddleware<LoggingMiddlewares>();
            app.Use(async (context, next) =>
            {
                if (!context.Request.Headers.ContainsKey("Index"))
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Musisz podaæ nr indeksu");
                    return;
                }
                string index = context.Request.Headers["Index"].ToString();
                var student = service.GetStudent(index);
                if (student == null)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Nie ma takiego numeru indeksu");
                    return;
                }
                await next();
            });

            app.UseMiddleware<CustomMiddlewear>();
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
