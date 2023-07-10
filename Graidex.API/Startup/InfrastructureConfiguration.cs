using Graidex.API.Infrastructure;
using Graidex.Domain.Interfaces;
using Graidex.Infrastructure.Data;
using Graidex.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

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
            
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<ITestResultRepository, TestResultRepository>();

            services.AddSingleton<IFileStorageProvider, FileStorageProvider>();

            return services;
        }

        public static WebApplication ConfigureDatabase(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                SeedData.EnsurePopulated(app
                    .Services.CreateScope()
                    .ServiceProvider.GetRequiredService<GraidexDbContext>());
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
