using AI_as_a_Service.Data;
using AI_as_a_Service.Helpers;
using AI_as_a_Service.Middlewares;
using AI_as_a_Service.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AI_as_a_Service.Services.Interfaces;
using AI_as_a_Service.Interfaces.Services;
using AI_as_a_Service.Services;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.AddConsole(); // Add console logging provider
builder.Logging.SetMinimumLevel(LogLevel.Information); // Set the minimum log level

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Email Service
builder.Services.AddScoped<EmailService>();

////SignalR
builder.Services.AddSignalR();

////SQL Server EntityFramework "Repository" class I made to make everything easier
builder.Services.AddScoped(typeof(IRepository<>), typeof(SQLServerRepository<>));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

////CosmosDB "Repository"
//builder.Services.AddScoped(typeof(IRepository<>), typeof(CosmosDbRepository<>));
//builder.Services.Configure<CosmosDbSettings>(builder.Configuration.GetSection("CosmosDbSettings"));
//builder.Services.AddSingleton(x => x.GetRequiredService<IOptions<CosmosDbSettings>>().Value);

//Services
builder.Services.AddScoped<IUserService, UsersService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IFineTuningService, FineTuningService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ITrainingService, TrainingService>();
builder.Services.AddScoped<IChatCompletionService, ChatCompletionService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IPlanService, PlanService>();

// Add OpenAI helper class
builder.Services.AddSingleton(new OpenAISDK(builder.Configuration["OpenAI:ApiKey"]));

// Add Configuration singleton
builder.Services.AddSingleton(Configuration.Instance); // Add this line to register the Configuration singleton

// Add StripeService
builder.Services.AddSingleton(new StripeSDK(builder.Configuration["Stripe:ApiKey"]));

// JIRA Cloud API Client
builder.Services.AddHttpClient<JiraApiClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["JiraApi:BaseUrl"]);
    client.DefaultRequestHeaders.Add("Authorization", $"Basic {builder.Configuration["JiraApi:EncodedCredentials"]}");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
.AddPolicyHandler(HttpPolicyExtensions.HandleTransientHttpError()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));


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
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();