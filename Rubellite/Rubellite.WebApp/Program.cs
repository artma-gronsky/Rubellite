using Rubellite.WebApp.Configurations;

#region Configure Services
var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterConfigurations(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.InitializeDatabase();
#endregion

app.Run();