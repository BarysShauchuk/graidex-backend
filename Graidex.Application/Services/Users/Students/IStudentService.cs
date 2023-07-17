using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Files;
using Graidex.Application.DTOs.Files.Images;
using Graidex.Application.DTOs.Users.Students;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users.Students
{
    public interface IStudentService
    {
        public Task<OneOf<StudentInfoDto, UserNotFound>> GetByEmailAsync(string email);

        public Task<OneOf<StudentInfoDto, UserNotFound>> GetCurrentAsync();

        public Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentInfoAsync(StudentInfoDto studentInfo);

        public Task<OneOf<Success, ValidationFailed, UserNotFound>> UpdateCurrentProfileImageAsync(UploadImageDto imageDto);

        public Task<OneOf<DownloadFileDto, UserNotFound, NotFound>> DownloadCurrentProfileImageAsync();

        public Task<OneOf<Success, UserNotFound>> DeleteCurrentProfileImageAsync();

        public Task<OneOf<Success, ValidationFailed, UserNotFound, WrongPassword>> UpdateCurrentPasswordAsync(ChangePasswordDto passwords);

        public Task<OneOf<Success, UserNotFound, WrongPassword>> DeleteCurrentAsync(string password);

        public Task<OneOf<List<StudentDto>, NotFound>> GetAllOfSubjectAsync(int subjectId);

        public Task<OneOf<DownloadFileDto, NotFound>> GetAllProfileImagesOfSubjectAsync(int subjectId);

        public Task<OneOf<Success, UserNotFound, NotFound>> RemoveFromSubjectAsync(int subjectId, string studentEmail);
    }
}
