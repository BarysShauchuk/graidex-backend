using Graidex.Application.DTOs.SubjectRequest;
using Graidex.Application.OneOfCustomTypes;
using OneOf.Types;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Subjects
{
    public interface ISubjectRequestService
    {
        public Task<OneOf<Success, UserNotFound, NotFound, UserAlreadyExists>> CreateRequestAsync(int subjectId, string studentEmail);
        public Task<OneOf<List<IncomingSubjectRequestDto>, UserNotFound>> GetAllOfCurrentAsync();
        public Task<OneOf<List<OutgoingSubjectRequestDto>, NotFound>> GetAllBySubjectIdAsync(int subjectId);
        public Task<OneOf<Success, UserNotFound, UserAlreadyExists, NotFound>> JoinSubjectByRequestIdAsync(int subjectRequestId);
        public Task<OneOf<Success>> RejectRequestByIdAsync(int subjectRequestId);
        public Task<OneOf<Success>> DeleteByIdAsync(int subjectRequestId);
    }
}
