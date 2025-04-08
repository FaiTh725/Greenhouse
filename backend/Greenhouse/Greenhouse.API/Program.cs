using Greenhouse.API.Extentions;
using Greenhouse.API.Middlewares;
using Greenhouse.Application;
using Greenhouse.Dal;
using Greenhouse.Infastructure;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services
    .ConfigureAppServices(builder.Configuration)
    .ConfigureDalServices(builder.Configuration)
    .ConfigureApplicationServices()
    .ConfigureInfastructureServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("Client");

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();
app.UseExceptionHandler();

app.MapControllers();


app.Run();
