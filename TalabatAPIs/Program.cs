
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastrucure;
using Talabat.Infrastrucure.Data;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;
using TalabatAPIs.MiddleWares;

namespace TalabatAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webApplicationBuilder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Services
            webApplicationBuilder.Services.AddControllers();
            // Register Required Web APIs Services To The DI Container 


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            webApplicationBuilder.Services.AddEndpointsApiExplorer();
            webApplicationBuilder.Services.AddSwaggerGen();

            webApplicationBuilder.Services.AddDbContext<StoreContext>(options =>
            options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection"))
            );

            webApplicationBuilder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
           // webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            webApplicationBuilder.Services.AddAutoMapper(typeof(MappingProfiles));
            webApplicationBuilder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                    .SelectMany(P => P.Value.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToList();
                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });
            #endregion


            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();


            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            // Ask CLR for Creating Object from DbContext EXplicitly 
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();
            try
            {
                await _dbContext.Database.MigrateAsync(); // Update-Database
                await StoreContextSeed.SeedAsync(_dbContext);
            }
            catch (Exception ex)
            {

              
                logger.LogError(ex, "An Error Has Been Occuerd During aplly the Migration ");
                Console.WriteLine(ex);

            }
            #region Configur Kestrel MiddleWares
            app.UseMiddleware<ExeptionMiddleware>();
            #region way 3 for implement Middleware
            //app.Use(async (httpContext, _next) =>
            //{
            //    try
            //    {
            //        // Take an Action With The Request
            //        await _next.Invoke(httpContext); // Go To the Next Middleware

            //        // Take an Action with The Response
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex.Message); // Development Env 
            //                                      //log Exception in (database | Files) // Production Env
            //        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            //        var response = app.Environment.IsDevelopment() ?
            //            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace)
            //            :
            //            new ApiExceptionResponse((int)HttpStatusCode.InternalServerError);
            //        var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            //        var json = JsonSerializer.Serialize(response, options);
            //        await httpContext.Response.WriteAsync(json);


            //    }

            //}); 
            #endregion
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // app.UseRouting();
            //app.UseEndPoints(endPoints => 
            //{ 
            //endPoints.MapControllerRoute (
            //      name : "default" ,
            //      pattern : "{controller}/{action}/{id}"
            //);
            //endPoints.MapController();
            //}


            app.MapControllers();
            #endregion



            app.Run();
        }
    }
}
