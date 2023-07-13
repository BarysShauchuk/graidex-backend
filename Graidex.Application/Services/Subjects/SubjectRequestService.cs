using AutoMapper;
using Graidex.Application.Interfaces;
using Graidex.Application.DTOs.SubjectRequest;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using OneOf;
using OneOf.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graidex.Domain.Models;
using Graidex.Domain.Models.Users;
using System.Security.Cryptography.X509Certificates;
using Graidex.Application.DTOs.Subject;

namespace Graidex.Application.Services.Subjects
{
    public class SubjectRequestService : ISubjectRequestService
    {
        private readonly ICurrentUserService currentUser;
        private readonly ITeacherRepository teacherRepository;
        private readonly IStudentRepository studentRepository;
        private readonly ISubjectRepository subjectRepository;
        private readonly ISubjectRequestRepository subjectRequestRepository;
        private readonly IMapper mapper;
        public SubjectRequestService(
            ICurrentUserService currentUser,
            ITeacherRepository teacherRepository,
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository,
            ISubjectRequestRepository subjectRequestRepository,
            IMapper mapper) 
        {
            this.currentUser = currentUser;
            this.teacherRepository = teacherRepository;
            this.studentRepository = studentRepository;
            this.subjectRepository = subjectRepository;
            this.subjectRequestRepository = subjectRequestRepository;
            this.mapper = mapper;
        }
        public async Task<OneOf<Success, ValidationFailed, UserNotFound, NotFound>> CreateRequestAsync(int subjectId, OutgoingSubjectRequestDto outgoingRequest)
        {   
            // TODO: Add validation

            string teacherEmail = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(teacherEmail);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }
            
            string studentEmail = outgoingRequest.StudentEmail;
            var student = await this.studentRepository.GetByEmail(studentEmail);
            if (student is null)
            {
                return new UserNotFound($"Student with email \"{studentEmail}\" is not found.");
            }

            var subject = await this.subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            if (subject.Students.Any(s => s.Id == student.Id))
            {
                return new UserNotFound($"Student with email \"{studentEmail}\" is already registered to this subject.");
            }

            if (!subjectRequestRepository.GetAll().Any(x => x.SubjectId == subjectId && x.StudentId == student.Id))
            {
                var subjectRequest = this.mapper.Map<SubjectRequest>(outgoingRequest);
                subjectRequest.SubjectId = subjectId;
                subjectRequest.StudentId = student.Id;
                subjectRequest.Date = DateTime.UtcNow;
                await this.subjectRequestRepository.Add(subjectRequest);
            }

            return new Success();
        }

        public async Task<OneOf<List<IncomingSubjectRequestDto>, UserNotFound>> GetAllOfCurrentAsync()
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subjectRequests = this.subjectRequestRepository.GetAll().Where(x => x.StudentId == student.Id);
            var subjectRequestDtos = new List<IncomingSubjectRequestDto>();
            foreach (var request in subjectRequests)
            {
                IncomingSubjectRequestDto requestDto = this.mapper.Map<IncomingSubjectRequestDto>(request);
                var subject = await this.subjectRepository.GetById(request.SubjectId);
                requestDto.SubjectInfo = this.mapper.Map<SubjectInfoDto>(subject);
                subjectRequestDtos.Add(requestDto);
            }
            return subjectRequestDtos;
        }

        public async Task<OneOf<List<SubjectRequestInfoDto>, UserNotFound, NotFound>> GetAllBySubjectIdAsync(int subjectId)
        {
            string email = this.currentUser.GetEmail();

            var teacher = await this.teacherRepository.GetByEmail(email);
            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subject = await this.subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            var subjectRequests = this.subjectRequestRepository.GetAll().Where(x => x.SubjectId == subject.Id);
            var subjectRequestDtos = new List<SubjectRequestInfoDto>();
            foreach (var request in subjectRequests)
            {
                
                var student = await this.studentRepository.GetById(request.StudentId);
                SubjectRequestInfoDto requestDto = this.mapper.Map<SubjectRequestInfoDto>(student);
                requestDto.Date = request.Date;
                subjectRequestDtos.Add(requestDto);
            }
            return subjectRequestDtos;
        }

        public async Task<OneOf<Success, UserNotFound, NotFound>> JoinSubjectByRequestIdAsync(int requestId)
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subjectRequest = await this.subjectRequestRepository.GetById(requestId);
            if (subjectRequest is null)
            {
                return new NotFound();
            }

            if(subjectRequest.StudentId != student.Id)
            {
                return new UserNotFound(
                    $"Student with email \"{email}\" is not invited by request with id \"{requestId}\"");
            }

            var subject = await this.subjectRepository.GetById(subjectRequest.SubjectId);
            if (subject is null)
            {
                return new NotFound();
            }
            subject.Students.Add(student);
            await this.subjectRepository.Update(subject);
            await this.subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }

        public async Task<OneOf<Success, UserNotFound, NotFound>> RejectRequestByIdAsync(int requestId)
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);
            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subjectRequest = await this.subjectRequestRepository.GetById(requestId);
            if (subjectRequest is null)
            {
                return new NotFound();
            }

            if (subjectRequest.StudentId != student.Id)
            {
                return new UserNotFound(
                    $"Student with email \"{email}\" is not invited by request with id \"{requestId}\"");
            }

            await subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }

        public async Task<OneOf<Success, UserNotFound, NotFound>> DeleteByIdAsync(int id)
        {
            string email = this.currentUser.GetEmail();
            var teacher = await this.teacherRepository.GetByEmail(email);

            if (teacher is null)
            {
                return this.currentUser.UserNotFound("Teacher");
            }

            var subjectRequest = await this.subjectRequestRepository.GetById(id);
            if (subjectRequest is null)
            {
                return new NotFound();
            }

            await subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }
    }
}
