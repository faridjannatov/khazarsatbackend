using CansatGround;
using CansatGround.Models;
using CansatGround.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Configuration.AddJsonFile("cansatprops.json", optional: false, reloadOnChange: true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSignalR(opts => opts.EnableDetailedErrors = true);
builder.Services.AddSingleton<XbeeService>();
builder.Services.AddSingleton<CSVService>();
builder.Services.AddScoped<CPropsService>();


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

CansatHelper.AddServices(builder.Services, builder.Configuration);

var app = builder.Build();

// var csvService = app.Services.GetRequiredService<CSVService>();
// csvService.testcsvwrite();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors();

//app.MapHub<TelemetryHub>("/telemetry");

app.MapControllers();

app.Run();