using FluentValidation;
using Graidex.API.WebServices;
using Graidex.Application;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Authorization.PolicyHandlers;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Application.Services.Subjects;
using Graidex.Application.Services.Users.Students;
using Graidex.Application.Services.Users.Teachers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Graidex.API.Startup
{
    public static class DependencyInjectionSetup
    {
        public static IServiceCollection RegisterHostedServices(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection RegisterAuthenticationServices(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddScoped<IStudentAuthenticationService, AuthenticationService>();
            services.AddScoped<ITeacherAuthenticationService, AuthenticationService>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            System.Text.Encoding.UTF8.GetBytes(
                                configuration.GetSection("AppSettings:Token").Value!)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    });

            return services;
        }

        public static IServiceCollection RegisterAuthorizationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, IsTeacherOfSubjectHandler>();
            services.AddScoped<IAuthorizationHandler, IsStudentOfSubjectHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfStudentHandler>();
            services.AddScoped<IAuthorizationHandler, IsStudentOfTeacherHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("TeacherOfSubject", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfSubjectRequirement());
                });

                options.AddPolicy("StudentOfSubject", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfSubjectRequirement());
                });

                options.AddPolicy("TeacherOfStudent", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfStudentRequirement());
                });

                options.AddPolicy("StudentOfTeacher", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfTeacherRequirement());
                });
            });

            return services;
        }

        public static IServiceCollection RegisterWebServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IRouteDataService, RouteDataService>();

            return services;
        }

        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITeacherService, TeacherService>();

            services.AddScoped<ISubjectService, SubjectService>();

            services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
            services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);

            return services;
        }
    }
}
