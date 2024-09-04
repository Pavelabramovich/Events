using Microsoft.EntityFrameworkCore;
using Events.WebApi.Db;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using System;
using Events.WebApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;
configuration.AddJsonFile("secrets.json");


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
    });

builder.Services.AddDbContext<EventsContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});




builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => 
    {
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;

      //  options.SignIn.RequireConfirmedEmail = true;
    })
    .AddEntityFrameworkStores<EventsContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>   // задаёт схему аутентификации по умолчанию
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>  // настройка аутентификации на основе JWT
    {
        var Key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);

        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["JWT:Issuer"],
            ValidAudience = configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Key),
            ClockSkew = TimeSpan.Zero
        };
    });



builder.Services.AddScoped<IAuthorizationHandler, RoleRequirementHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.AddRequirements(new RoleRequirement("Admin")));
});



builder.Services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();
builder.Services.AddScoped<IUserServiceRepository, UserServiceRepository>();











// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();








// builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(setup => 
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Events API",
        Version = "v1"
    });
    setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" + 
                      "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" + 
                      "Example: \"Bearer 1safsfsdfdfd\"",
    });
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement() 
    {
        [
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference() 
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }
        ] = []
    });
});




var app = builder.Build();

// Configure the HTTP request pipeline.
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





app.UseAuthorization();
app.MapControllers();


app.Run();
