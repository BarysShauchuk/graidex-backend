using FluentValidation;
using Graidex.API.WebServices;
using Graidex.Application;
using Graidex.Application.AutoMapperProfiles;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Authorization.PolicyHandlers;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Application.Services.Subjects;
using Graidex.Application.Services.Users;
using Graidex.Application.Services.Users.Students;
using Graidex.Application.Services.Users.Teachers;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using Graidex.Infrastructure.Data;
using Graidex.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Runtime.InteropServices;

var builder = WebApplication.CreateBuilder(args);

var frontendUrl = Environment.GetEnvironmentVariable("GRAIDEX_FRONTEND_URL")
    ?? builder.Configuration.GetSection("AppSettings:FrontendUrl").Value!;

var dbConnectionString = Environment.GetEnvironmentVariable("GRAIDEX_DB_CONNECTIONSTRING")
    ?? builder.Configuration.GetConnectionString("GraidexDb");

builder.Services.AddControllers();
builder.Services.AddDbContext<GraidexDbContext>(options =>
    options
    .UseLazyLoadingProxies()
    .UseSqlServer(dbConnectionString));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<ITestRepository, TestRepository>();
builder.Services.AddScoped<ITestResultRepository, TestResultRepository>();

builder.Services.AddScoped<IStudentAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITeacherAuthenticationService, AuthenticationService>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IRouteDataService, RouteDataService>();

builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();

builder.Services.AddScoped<ISubjectService, SubjectService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddHttpContextAccessor();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(
                    builder.Configuration.GetSection("AppSettings:Token").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        });

builder.Services.AddScoped<IAuthorizationHandler, IsTeacherOfSubjectHandler>();
builder.Services.AddScoped<IAuthorizationHandler, IsStudentOfSubjectHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("TeacherOfSubject", policyBuilder =>
    {
        policyBuilder.RequireRole("Teacher");
        policyBuilder.AddRequirements(new IsTeacherOfSubjectRequirement());
    });

    options.AddPolicy("StudentOfSubject", policyBuilder =>
    {
        policyBuilder.RequireRole("Student");
        policyBuilder.AddRequirements(new IsStudentOfSubjectRequirement());
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
builder.Services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);
builder.Services.AddCors();

var app = builder.Build();

SeedData.EnsureMigrated(
    app.Services.CreateScope().ServiceProvider.GetRequiredService<GraidexDbContext>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    SeedData.EnsurePopulated(
        app.Services.CreateScope().ServiceProvider.GetRequiredService<GraidexDbContext>());
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins(frontendUrl));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
