using FluentValidation;
using Graidex.API.WebServices;
using Graidex.Application;
using Graidex.Application.Factories.Answers;
using Graidex.Application.Factories.Tests;
using Graidex.Application.Interfaces;
using Graidex.Application.Services.Authentication;
using Graidex.Application.Services.Authorization.PolicyHandlers.Student;
using Graidex.Application.Services.Authorization.PolicyHandlers.Teacher;
using Graidex.Application.Services.Authorization.Requirements;
using Graidex.Application.Services.Authorization.Requirements.Student;
using Graidex.Application.Services.Authorization.Requirements.Teacher;
using Graidex.Application.Services.Subjects;
using Graidex.Application.Services.TestChecking;
using Graidex.Application.Services.TestChecking.AnswerCheckers;
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
            services.AddScoped<IAuthorizationHandler, IsStudentOfTestResultHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfSubjectContentHandler>();
            services.AddScoped<IAuthorizationHandler, IsTeacherOfTestResultHandler>();

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

                options.AddPolicy("StudentOfTestResult", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Student");
                    policyBuilder.AddRequirements(new IsStudentOfTestResultRequirement());
                });

                options.AddPolicy("TeacherOfSubjectContent", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfSubjectContentRequirement());
                });

                options.AddPolicy("TeacherOfTestResult", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.RequireRole("Teacher");
                    policyBuilder.AddRequirements(new IsTeacherOfTestResultRequirement());
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

            services.AddSingleton<
                Microsoft.AspNetCore.StaticFiles.IContentTypeProvider, 
                FileExtensionContentTypeProvider
                >();

            services.AddSingleton<
                Graidex.Application.Interfaces.IContentTypeProvider, 
                ContentTypeProvider
                >();

            return services;
        }

        public static IServiceCollection RegisterFactories(this IServiceCollection services)
        {
            services.AddScoped<IAnswerFactory, AnswerFactory>();
            services.AddScoped<ITestBaseFactory, TestBaseFactory>();

            return services;
        }

        public static IServiceCollection RegisterTestCheckingServices(this IServiceCollection services)
        {
            services.AddSingleton<ITestCheckingService, TestCheckingService>();

            services.AddSingleton<IAnswerChecker, SingleChoiceAnswerChecker>();
            services.AddSingleton<IAnswerChecker, MultipleChoiceAnswerChecker>();

            return services;
        }
    }
}
