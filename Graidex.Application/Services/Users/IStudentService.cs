using Graidex.Application.DTOs.Authentication;
using Graidex.Application.DTOs.Users;
using Graidex.Application.Infrastructure.ResultObjects.Generic;
using Graidex.Application.Infrastructure.ResultObjects.NonGeneric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users
{
    public interface IStudentService
    {
        public Task<Result<StudentInfoDto>> GetByEmail(string email);

        public Task<Result<StudentInfoDto>> GetCurrent();

        public Task<Result> UpdateCurrentInfo(StudentInfoDto studentInfo);

        public Task<Result> UpdateCurrentPassword(ChangePasswordDto passwords);

        public Task<Result> DeleteCurrent(string password);
    }
}
