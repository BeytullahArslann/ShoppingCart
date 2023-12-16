using ShoppingCart.Application.Services;
using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Infrastructure.Services;
using ShoppingCart.Presentation.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace ShoppingCart.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<IDiscountService, DiscountService>();
        services.AddExceptionHandler<CustomExceptionHandler>();

        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ShoppingCart API",
                Version = "v1"
            });

            c.DocInclusionPredicate((_, _) => true);
        });

        return services;
    }
}