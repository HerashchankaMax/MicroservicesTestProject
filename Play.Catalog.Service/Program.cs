using System.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Repositories;
using Play.Catalog.Service.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeSerializer(BsonType.String));
var configuration = builder.Configuration;
var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
builder.Services.AddSingleton(provider =>
{
    var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
    string connectionString = mongoSettings.ConnectionString;
    var client = new MongoClient(connectionString);
    return client.GetDatabase(serviceSettings.ServiceName);
});
builder.Services.AddSingleton<IItemsRepository, ItemsRepository>();
builder.Services.AddControllers(options =>
    options.SuppressAsyncSuffixInActionNames = false);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
