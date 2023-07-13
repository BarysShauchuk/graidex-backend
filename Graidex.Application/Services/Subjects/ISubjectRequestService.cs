using Graidex.Application.DTOs.SubjectRequest;
using Graidex.Application.OneOfCustomTypes;
using OneOf.Types;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Subjects
{
    public interface ISubjectRequestService
    {
        public Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> CreateRequestAsync(int id, OutgoingSubjectRequestDto outgoingRequest);
        public Task<OneOf<List<IncomingSubjectRequestDto>, UserNotFound>> GetAllOfCurrentAsync();
        public Task<OneOf<List<SubjectRequestInfoDto>, UserNotFound, NotFound>> GetAllBySubjectIdAsync(int subjectId);
        public Task<OneOf<Success, UserNotFound, NotFound>> JoinSubjectByRequestIdAsync(int requestId);
        public Task<OneOf<Success, UserNotFound, NotFound>> RejectRequestByIdAsync(int requestId);
        public Task<OneOf<Success, UserNotFound, NotFound>> DeleteByIdAsync(int id);
    }
}
