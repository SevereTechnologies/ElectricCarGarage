using ApplicationBlocks.Behaviors;
using BookingGateway.Domain.Entities;
using BookingGateway.Infrastructure.Repositories;
using DiscountGateway.Presentation.gRPC.Protos;

var builder = WebApplication.CreateBuilder(args);


// Add services to the containter==========================================
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<Booking>().Identity(x => x.CustomerId);
}).UseLightweightSessions();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.Decorate<IBookingRepository, CachedBookingRepository>(); // Use Scrutor to use a cached version of BookingRepository

//StackExchangeRedis ===========================
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

//Grpc Services ===============================
builder.Services.AddGrpcClient<DiscountService.DiscountServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]!);
})
.ConfigurePrimaryHttpMessageHandler(() => // accept any certificate in DEV environment so we don't get SSL Certificate Exception
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };

    return handler;
});


var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
