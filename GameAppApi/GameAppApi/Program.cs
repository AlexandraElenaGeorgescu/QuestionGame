using Microsoft.Extensions.Options;
using GameAppApi.API.DatabaseSettings;
using Microsoft.OpenApi.Models;
using System.Reflection;
using GameAppApi.Game.Services;
using GameAppApi.Authentification.PublicServices;
using GameAppApi.UserAdministration.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// First, configure MongoDBSettings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddSingleton<IMongoDBSettings>(sp => sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

// Register your UserService
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IAdminService, AdminService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "CorsPolicy",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });
});

// Add MVC Controllers if you have any
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true); // The second parameter 'true' will suppress errors if the file is not found
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

// Seed the moderator account
SeedModerator(app.Services, builder.Configuration);

app.Run();

// Define the SeedModerator method
static void SeedModerator(IServiceProvider services, IConfiguration configuration)
{
    using var scope = services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var moderatorUsername = configuration["ModeratorSettings:Username"];
    var moderatorPassword = configuration["ModeratorSettings:Password"];

    userService.SeedModerator(moderatorUsername, moderatorPassword);
}
