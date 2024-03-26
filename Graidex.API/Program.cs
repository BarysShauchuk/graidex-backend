using Graidex.API.Hubs;
using Graidex.API.Startup;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSignalR().AddJsonProtocol(options =>
{
    options.PayloadSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.RegisterInfrastructureServices(builder.Configuration);

builder.Services.RegisterAuthenticationServices(builder.Configuration);
builder.Services.RegisterAuthorizationServices();

builder.Services.RegisterWebServices();
builder.Services.RegisterApplicationServices();
builder.Services.RegisterSchedulerServices(builder.Configuration);
builder.Services.RegisterNotificationsServices();
builder.Services.RegisterFactories();
builder.Services.RegisterTestCheckingServices();

builder.Services.RegisterSwaggerServices();

var app = builder.Build();

app.ConfigureDatabase();
app.ConfigureStorageStructure();

app.ConfigureSwagger();

app.UseHttpsRedirection();
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyHeader();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowCredentials();
    policyBuilder.WithOrigins(
        builder.Configuration.GetRequiredSection("AppSettings:FrontendUrl").Value!);
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationsHub>("/notifications", options =>
{
    options.CloseOnAuthenticationExpiration = true;
});

app.Run();
