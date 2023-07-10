using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Domain.Interfaces
{
    public interface IFileStorageProvider
    {
        public Task UploadAsync(Stream stream, string fileName, string path);

        public Task<Stream> DownloadAsync(string fileName, string path);

        public Task DeleteAsync(string fileName, string path);
    }
}
