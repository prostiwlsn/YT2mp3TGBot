using Microsoft.Extensions.Configuration;
using YT2mp3.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMessageHandler, MessageHandler>();

using (HttpClient http = new HttpClient())
{
    string token = builder.Configuration.GetValue<string>("BotToken")!;
    string host = builder.Configuration.GetValue<string>("ServerIP")!;
    string url = $"https://api.telegram.org/bot{token}/setWebhook?url={host}";
    Console.WriteLine(url);

    HttpContent content = new StringContent("");
    var response = await http.PostAsync(url, content);
    Console.WriteLine(response.StatusCode);
}

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
