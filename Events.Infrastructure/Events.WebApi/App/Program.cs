using System.Text.Json.Serialization;
using Events.WebApi.App;
using Events.Application.Dto;
using Events.WebApi.Extensions;


var builder = WebApplication.CreateBuilder(args);


builder.AddConfiguration();

builder.Services.AddAutoMapper<EventsMapperProfile>();


builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    });


builder.AddValidation();


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
