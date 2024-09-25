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

//app.UseExceptionHandler();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseExceptionHandler();

app.UseRouting();

app.UseAuthentication().UseAuthorization();

//app.Use(async (context, next) =>
//{
//    try
//    {
//        await next(context);
//    }
//    catch (Exception exception)
//    {
//        context.Response.StatusCode = 500;
//    }
//});



app.MapControllers();

app.Run();
