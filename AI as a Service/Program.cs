using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.AddConsole(); // Add console logging provider
builder.Logging.SetMinimumLevel(LogLevel.Information); // Set the minimum log level

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add OpenAI API configuration
//builder.Services.AddSingleton(new OpenAI.ApiClient(builder.Configuration["OpenAI:ApiKey"]));

// Add OpenAI helper class
//builder.Services.AddSingleton(new OpenAI(builder.Configuration["OpenAI:ApiKey"]));

// Add Configuration singleton
builder.Services.AddSingleton(Configuration.Instance); // Add this line to register the Configuration singleton

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
});


app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();