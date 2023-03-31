using AI_as_a_Service.Data;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AI_as_a_Service.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.AddConsole(); // Add console logging provider
builder.Logging.SetMinimumLevel(LogLevel.Information); // Set the minimum log level

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////SignalR
//builder.Services.AddSignalR();

//// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////SQL Server EntityFramework "Repository" class I made to make everything easier
//builder.Services.AddScoped(typeof(IRepository<>), typeof(SQLServerRepository<>));

////CosmosDB "Repository"
//builder.Services.AddScoped(typeof(IRepository<>), typeof(CosmosDbRepository<>));
////And configure the CosmosDbSettings class
//builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection("CosmosDbSettings"));
//builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<CosmosDbSettings>>().Value);


// Add OpenAI helper class
//builder.Services.AddSingleton(new OpenAI(builder.Configuration["OpenAI:ApiKey"]));

// Add Configuration singleton
builder.Services.AddSingleton(Configuration.Instance); // Add this line to register the Configuration singleton

//If we do this then we don't need to use it in the constructur, but this is wrong I believe // Add StripeService
//builder.Services.AddSingleton(new StripeSDK(builder.Configuration["Stripe:ApiKey"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
// Add the authentication middleware before the authorization middleware
app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    //SignalR Server
    //endpoints.MapHub<ChatHub>("/chatHub");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();