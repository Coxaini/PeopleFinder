using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using PeopleFinder.Api;
using PeopleFinder.Api.Hubs;
using PeopleFinder.Api.Middlewares;
using PeopleFinder.Application;
using PeopleFinder.Infrastructure;
using PeopleFinder.Infrastructure.Persistence;
using PeopleFinder.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 1024 * 1024 * 100; // 100MB
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithAuthentication();

builder.Services.AddApplicationInsightsTelemetry();

builder.Services
    .AddMappings()
    .AddApplication(builder.Configuration)
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(options => options.AddPolicy(name: "NgOrigins",
    policy =>
    {
        policy.WithOrigins("https://localhost:3000")
            .AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("*","X-Pagination");
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithExposedHeaders("*","X-Pagination");
        
    }));

//add configuration for files 
builder.Configuration.AddJsonFile("filesettings.json");

builder.Services.AddSignalR();

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    var dbcontext = app.Services.CreateScope().ServiceProvider.GetRequiredService<PeopleFinderDbContext>();
//dbcontext.Database.EnsureDeleted();
    dbcontext.Database.EnsureCreated();
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}
app.UseCors("NgOrigins");

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseMiddleware<AuthTokenSetterMiddleware>();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapHub<ChatHub>("chats");

app.Run();
