using FluentValidation;
using Graidex.API.HostedServices;
using Graidex.API.WebServices;
using Graidex.Application;
using Graidex.Application.Factories;
using Graidex.Application.Interfaces;
using Graidex.Application.Interfaces.TestCheckingQueue;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Authorization.PolicyHandlers;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Application.Services.Subjects;
using Graidex.Application.Services.Tests;
using Graidex.Application.Services.Tests.TestChecking;
using Graidex.Application.Services.Users.Students;
using Graidex.Application.Services.Users.Teachers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;

namespace Graidex.API.Startup
{
    public static class DependencyInjectionSetup
    {
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
                                configuration.GetRequiredSection("AppSettings:Token").Value!)),
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
            services.AddScoped<IAuthorizationHandler, IsStudentOfRequestHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfRequestHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfDraftHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfTestHandler>();
            services.AddScoped<IAuthorizationHandler, IsStudentOfVisibleTestHandler>();
            services.AddScoped<IAuthorizationHandler, IsStudentOfAttemptHandler>();

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

                options.AddPolicy("StudentOfRequest", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfRequestRequirement());
                });

                options.AddPolicy("TeacherOfRequest", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfRequestRequirement());
                });

                options.AddPolicy("TeacherOfDraft", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfDraftRequirement());
                });

                options.AddPolicy("TeacherOfTest", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfTestRequirement());
                });

                options.AddPolicy("StudentOfVisibleTest", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfVisibleTestRequirement());
                });

                options.AddPolicy("StudentOfAttempt", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfAttemptRequirement());
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
            services.AddScoped<ITestService, TestService>();
            services.AddScoped<ITestResultService, TestResultService>();

            services.AddScoped<ISubjectRequestService, SubjectRequestService>();

            services.AddValidatorsFromAssemblyContaining<IApplicationAssemblyMarker>();
            services.AddAutoMapper(typeof(IApplicationAssemblyMarker).Assembly);

            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

            return services;
        }

        public static IServiceCollection RegisterFactories(this IServiceCollection services)
        {
            services.AddScoped<IAnswerFactory, AnswerFactory>();

            return services;
        }

        public static IServiceCollection RegisterTestCheckingServices(this IServiceCollection services)
        {
            //services.AddHostedService<TestCheckingBackgroundService>();
            services.RegisterTestCheckingQueue();
            services.AddSingleton<ITestCheckingService, TestCheckingService>();

            return services;
        }

        private static IServiceCollection RegisterTestCheckingQueue(this IServiceCollection services)
        {
            services.AddSingleton<TestCheckingQueue>();

            services.AddSingleton<ITestCheckingInQueue>(
                sp => sp.GetRequiredService<TestCheckingQueue>());

            services.AddSingleton<ITestCheckingOutQueue>(
                sp => sp.GetRequiredService<TestCheckingQueue>());

            return services;
        }
    }
}
