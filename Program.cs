using aeternaCaptions.src.Database;
using aeternaCaptions.src.Interface;
using aeternaCaptions.src.model;
using aeternaCaptions.src.Repository;
using aeternaCaptions.src.Utils;
using Microsoft.EntityFrameworkCore;
using System;


var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

//CONFIG
StaticConfiguration.Initialize(
    Environment.GetEnvironmentVariable("ASSEMBLYAI_API_KEY")!, 
    Environment.GetEnvironmentVariable("DEEPL_API_KEY")!,
    Configuration);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDBContext>(options => {
    options.UseNpgsql(Environment.GetEnvironmentVariable("CONN_STRING"));    
    
});
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddScoped<ISubtitleFileRepo, SubtitleFileRepo>();
builder.Services.AddScoped<ITranscriptRepo, TranscriptRepo>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

