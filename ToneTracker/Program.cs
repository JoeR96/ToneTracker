using ToneTracker;
using ToneTracker.Filters;
using ToneTracker.Pedal;
using ToneTracker.Tone;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("ToneTracker") ?? "ToneTracker.db";
builder.Services.AddSqlite<ToneTrackerDbContext>(connectionString);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<GuidSchemaFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Map("/", () => Results.Redirect("/swagger"));

// Configure the APIs
app.MapPedals();
app.MapAmplifiers();
app.MapTones();
app.Run();

public partial class Program { }
