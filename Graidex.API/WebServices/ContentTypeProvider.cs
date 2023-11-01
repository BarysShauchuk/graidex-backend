using Microsoft.AspNetCore.StaticFiles;

namespace Graidex.API.WebServices
{
    public class ContentTypeProvider : Graidex.Application.Interfaces.IContentTypeProvider
    {
        private readonly IContentTypeProvider contentTypeProvider;

        public ContentTypeProvider(IContentTypeProvider contentTypeProvider)
        {
            this.contentTypeProvider = contentTypeProvider;
        }

        public string? GetContentType(string fileName)
        {
            if (contentTypeProvider.TryGetContentType(fileName, out var contentType))
            {
                return contentType;
            }

            return null;
        }
    }
}
