using Kodla.Api;
using Kodla.Api.RequestBookingMessage;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.AddKafkaProducer<string, BookingRequestMessage>("kafka", producerBuilder => {
    producerBuilder.SetValueSerializer(new SimpleJsonKafkaSerializer<BookingRequestMessage>());
});

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();
