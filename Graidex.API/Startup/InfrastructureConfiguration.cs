using Graidex.API.Infrastructure;
using Graidex.Domain.Interfaces;
using Graidex.Infrastructure.Configuration;
using Graidex.Infrastructure.Data;
using Graidex.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Graidex.API.Startup
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection RegisterInfrastructureServices(
            this IServiceCollection services,
            ConfigurationManager configuration)
        {
            services.AddDbContext<GraidexDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("GraidexDb"));
                options.UseLazyLoadingProxies();
            });


            services.Configure<MongoDbConfig>(
                configuration.GetSection("AppSettings").GetSection("MongoDb"));

            services.AddSingleton<GraidexMongoDbClient>(serviceProvider =>
                new GraidexMongoDbClient(
                    configuration.GetConnectionString("GraidexDb.MongoDb"),
                    serviceProvider.GetRequiredService<IOptions<MongoDbConfig>>(),
                    serviceProvider.GetRequiredService<ILogger<GraidexMongoDbClient>>())
            );

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ITestDraftRepository, TestDraftRepository>();
            services.AddScoped<ITestResultRepository, TestResultRepository>();
            services.AddScoped<ISubjectRequestRepository, SubjectRequestRepository>();

            services.AddScoped<ITestQuestionsRepository, TestQuestionsRepository>();

            services.AddSingleton<IFileStorageProvider, FileStorageProvider>();

            return services;
        }

        public static WebApplication ConfigureDatabase(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                var serviceProvider = app.Services.CreateScope().ServiceProvider;
                SeedData.EnsurePopulated(
                    serviceProvider.GetRequiredService<GraidexDbContext>(),
                    serviceProvider.GetRequiredService<GraidexMongoDbClient>());
            }

            return app;
        }

        public static WebApplication ConfigureStorageStructure(this WebApplication app)
        {
            var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
            var rootPath = environment.WebRootPath;

            string[] folders = 
            {
                "ProfileImages",
                    "ProfileImages/Students",
                    "ProfileImages/Teachers"
            };

            foreach (string folder in folders)
            {
                var path = Path.Combine(rootPath, folder);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }

            return app;
        }
    }
}
