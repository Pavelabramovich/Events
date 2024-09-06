using System.Text.Json.Serialization;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Events.Domain;
using Events.DataBase;
using Events.WebApi.App;


var builder = WebApplication.CreateBuilder(args);


builder.AddConfiguration();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    });


builder.AddAuthentication();
builder.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.AddExceptionMiddleware();
builder.AddSwagger();
builder.AddServices();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication(); // add before UseAuthorization() 
app.UseAuthorization();

app.MapControllers();

app.Run();
