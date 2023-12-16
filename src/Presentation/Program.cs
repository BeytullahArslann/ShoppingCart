using ShoppingCart.Application;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Data;
using ShoppingCart.Presentation;
using ShoppingCart.Presentation.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddServices();

var app = builder.Build();

await app.InitialiseDatabaseAsync();

app.UseHsts();
app.MapEndpoints();

app.UseSwagger();
app.UseSwaggerUI(settings => { settings.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart API"); });

app.UseExceptionHandler(options => { });


app.Run();