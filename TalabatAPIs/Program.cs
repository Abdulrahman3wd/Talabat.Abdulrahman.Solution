
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastrucure;
using Talabat.Infrastrucure.Data;
using TalabatAPIs.Helpers;

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
            #endregion


            var app = webApplicationBuilder.Build();

            using var scope = app.Services.CreateScope();


            var services = scope.ServiceProvider;

            var _dbContext = services.GetRequiredService<StoreContext>();
            // Ask CLR for Creating Object from DbContext EXplicitly

            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _dbContext.Database.MigrateAsync(); // Update-Database
                await StoreContextSeed.SeedAsync(_dbContext);
            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An Error Has Been Occuerd During aplly the Migration ");
                Console.WriteLine(ex);

            }
            #region Configur Kestrel MiddleWares
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

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
