using OfferingGateway.Presentation.Grpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container ===========================================================
var assembly = typeof(Program).Assembly;

builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    //config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<OfferService>();

// Configure the HTTP Request pipeline ======================================================
app.MapCarter(); //scan all code for ICarterModule implemention and map required http methods


app.Run();
