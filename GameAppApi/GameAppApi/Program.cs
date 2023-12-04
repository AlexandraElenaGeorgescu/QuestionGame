using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using GameAppApi.Authentification.DatabaseSettings;
using GameAppApi.Authentification.Services; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add MongoDB settings and services here
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddSingleton<IMongoDBSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDBSettings>>().Value);

// Add UserService to the container.
builder.Services.AddScoped<UserService>(); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Seed the moderator account
SeedModerator(app.Services, builder.Configuration);

app.Run();

// Define the SeedModerator method
static void SeedModerator(IServiceProvider services, IConfiguration configuration)
{
    using var scope = services.CreateScope();
    var userService = scope.ServiceProvider.GetRequiredService<UserService>();
    var moderatorUsername = configuration["ModeratorSettings:Username"];
    var moderatorPassword = configuration["ModeratorSettings:Password"];

    userService.SeedModerator(moderatorUsername, moderatorPassword);
}
