using Graidex.API.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

builder.Services.RegisterInfrastructureServices(builder.Configuration);

builder.Services.RegisterAuthenticationServices(builder.Configuration);
builder.Services.RegisterAuthorizationServices();

builder.Services.RegisterWebServices();
builder.Services.RegisterApplicationServices();
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
    policyBuilder.WithOrigins(
        builder.Configuration.GetRequiredSection("AppSettings:FrontendUrl").Value!);
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
