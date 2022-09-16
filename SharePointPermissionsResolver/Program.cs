using Microsoft.AspNetCore.Http.Features;
using SharePointPermissionsResolver.Models;
using SharePointPermissionsResolver.Services.AuthWrapper;
using SharePointPermissionsResolver.Services.SharePointService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<AzureADConfig>(builder.Configuration.GetSection("AzureAD"));
builder.Services.AddScoped<IAuthWrapper, AuthWrapper>();
builder.Services.AddScoped<ISharePointService, SharePointService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();

