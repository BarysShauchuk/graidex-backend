using Graidex.Application.DTOs.Subject;
using Graidex.Application.DTOs.Users;
using Graidex.Application.OneOfCustomTypes;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Subjects
{
    public interface ISubjectService
    {
        public Task<OneOf<SubjectDto, ValidationFailed, UserNotFound>> CreateForCurrentAsync(CreateSubjectDto subjectDto);
        public Task<OneOf<List<SubjectDto>, UserNotFound>> GetAllOfCurrentAsync();
        public Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetByIdAsync(int id);
        public Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> UpdateSubjectInfoAsync(int id, UpdateSubjectDto updateSubjectDto);
        public Task<OneOf<Success, UserNotFound, NotFound>> DeleteByIdAsync(int id);
    }
}
