using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Rubellite.WebApp.Configurations;

#region Configure Services
var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterConfigurations(builder.Configuration);
builder.Services.ConfigureProblemDetails();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(StatusCodeProblemDetails), StatusCodes.Status401Unauthorized));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(StatusCodeProblemDetails), StatusCodes.Status422UnprocessableEntity));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(StatusCodeProblemDetails), StatusCodes.Status424FailedDependency));
    options.Filters.Add(new ProducesResponseTypeAttribute(typeof(StatusCodeProblemDetails), StatusCodes.Status500InternalServerError));
    options.Filters.Add(new ProducesAttribute("application/json"));
});

builder.Services.ConfigureDatabase();

builder.Services.SetIdentityConfiguration();
builder.Services.SetAuthenticationConfiguration();
builder.Services.SetAuthorizationConfiguration();

builder.Services.RegisterRepositories();
builder.Services.RegisterServices();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterSwaggerGen();
#endregion

#region Configure
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseProblemDetails();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.InitializeDatabase();
#endregion

app.Run();