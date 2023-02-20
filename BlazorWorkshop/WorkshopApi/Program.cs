using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using WorkshopApi.Database;
using WorkshopApi.Utils;
using WorkshopConfTool.Shared.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ConferencesDbContext>(options =>
        options.UseInMemoryDatabase(databaseName: "ConfTool"));

builder.Services.AddMvc().AddFluentValidation(fv =>
        fv.RegisterValidatorsFromAssemblyContaining<ConferenceDetailsValidator>());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/BlazorWorkshop";
        options.RequireHttpsMetadata = false;
        options.Audience = "blazor-api";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DataGenerator.Initialize(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(o => true));

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
