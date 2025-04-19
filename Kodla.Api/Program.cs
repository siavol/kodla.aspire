using Kodla.Api.Clients;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddMassTransitRabbitMq("rabbitmq");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<MeetupProcessorClient>(
    static client => client.BaseAddress = new("https+http://meetup-processor-service"));

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
