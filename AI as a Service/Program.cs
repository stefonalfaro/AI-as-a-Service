using AI_as_a_Service.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add OpenAI API configuration
//builder.Services.AddSingleton(new OpenAI.ApiClient(builder.Configuration["OpenAI:ApiKey"]));

// Add OpenAI helper class
builder.Services.AddSingleton(new OpenAI(builder.Configuration["OpenAI:ApiKey"]));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
