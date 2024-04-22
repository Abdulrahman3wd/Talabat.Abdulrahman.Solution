using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using Talabat.Core.Repositories.Contract;
using Talabat.Infrastrucure;
using TalabatAPIs.Errors;
using TalabatAPIs.Helpers;

namespace TalabatAPIs.Extentions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplecationServices (this IServiceCollection services)
        {
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));


            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            services.AddAutoMapper(typeof(MappingProfiles));
            services.Configure<ApiBehaviorOptions>(options =>
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
            return services;

        }
    }
}
