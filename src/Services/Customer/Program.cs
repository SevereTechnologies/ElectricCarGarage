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


var app = builder.Build();


// Configure the HTTP Request pipeline ======================================================
app.MapCarter(); //scan all code for ICarterModule implemention and map required http methods


app.Run();