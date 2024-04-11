
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Talabat.Infrastrucure.Data;

namespace TalabatAPIs
{
    public class Program
    {
        public static void Main(string[] args)
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
            #endregion


            var app = webApplicationBuilder.Build();

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
