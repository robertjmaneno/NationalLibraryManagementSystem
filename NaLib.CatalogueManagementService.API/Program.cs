using NaLib.CatalogueManagementService.API.Services;
using NaLib.CatalogueManagementService.API.Seeders;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using NaLib.CatalogueManagementService.API.Configurations;
using NaLib.CatalogueManagementService.Lib.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add MongoDB settings
builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register IMongoDatabase as a Singleton
builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongoSettings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    var client = new MongoClient(mongoSettings.ConnectionString);
    return client.GetDatabase(mongoSettings.DatabaseName);
});

// Register your other services
builder.Services.AddSingleton<LibraryResourceService>();
builder.Services.AddSingleton<CreateLibraryResourceDto>();
builder.Services.AddSingleton<LibraryResourceSeeder>();
builder.Services.AddSingleton<UpdateLibraryResourceDto>();


// Add controllers
builder.Services.AddControllers();

// Add Swagger for development
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Seed the database
    using (var scope = app.Services.CreateScope())
    {
        var seeder = scope.ServiceProvider.GetRequiredService<LibraryResourceSeeder>();
        await seeder.SeedAsync();
    }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
