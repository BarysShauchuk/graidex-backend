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
        public Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetSubjectOfTeacherByIdAsync(int id);
        public Task<OneOf<SubjectInfoDto, UserNotFound, NotFound>> GetSubjectOfStudentByIdAsync(int id);
        public Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> UpdateSubjectInfoAsync(int id, UpdateSubjectDto updateSubjectDto);
        public Task<OneOf<Success, UserNotFound, NotFound>> DeleteByIdAsync(int id);
        public Task<OneOf<List<SubjectContentDto>, NotFound>> GetAllContentByIdAsync(int id);
        public Task<OneOf<List<SubjectContentDto>, UserNotFound, NotFound>> GetVisibleContentOfSubjectByIdAsync(int id);
        public Task<OneOf<Success, NotFound, ItemImmutable>> ChangeContentVisibilityByIdAsync(int contentId, bool isVisible);
        public Task<OneOf<Success, NotFound>> ChangeContentOrderIndexByIdAsync(int contentId, double orderIndex);
        public Task<OneOf<Success, NotFound>> RefreshContentOrderingByIdAsync(int subjectId);
    }
}
