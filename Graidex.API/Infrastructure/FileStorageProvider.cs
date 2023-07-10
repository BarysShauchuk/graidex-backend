using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.StaticFiles;

namespace Graidex.API.Infrastructure
{
    public class FileStorageProvider : IFileStorageProvider
    {
        private readonly string rootPath;

        public FileStorageProvider(
            IWebHostEnvironment environment)
        {
            rootPath = environment.WebRootPath;
        }

        public Task<Stream> DownloadAsync(string fileName, string path)
        {
            var filePath = Path.Combine(rootPath, path, fileName);
            var result = File.OpenRead(filePath);

            return Task.FromResult<Stream>(result);
        }

        public Task DeleteAsync(string fileName, string path)
        {
            File.Delete(Path.Combine(rootPath, path, fileName));
            return Task.CompletedTask;
        }

        public async Task UploadAsync(Stream stream, string fileName, string path)
        {
            var filePath = Path.Combine(rootPath, path, fileName);
            using var fileStream = File.Create(filePath);
            await stream.CopyToAsync(fileStream);
        }
    }
}
