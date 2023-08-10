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
        public async Task<OneOf<Success, UserNotFound, NotFound, UserAlreadyExists>> CreateRequestAsync(int subjectId, string studentEmail)
        {    
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
                return new UserAlreadyExists($"Student with email \"{studentEmail}\" is already registered to this subject.");
            }

            if (!subjectRequestRepository.GetAll().Any(x => x.SubjectId == subjectId && x.StudentId == student.Id))
            {
                var subjectRequest = new SubjectRequest
                {
                    StudentId = student.Id,
                    SubjectId = subjectId,
                    Date = DateTime.UtcNow
                };
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
            var subjectIds = subjectRequests.Select(x => x.SubjectId);

            var subjects = this.subjectRepository.GetAll().Where(x => subjectIds.Contains(x.Id)).ToList();

            var data = subjectRequests.ToList().Zip(subjects, (request, subject) => new { request = request, subject = subject });

            var subjectRequestDtos = new List<IncomingSubjectRequestDto>();

            foreach (var item in data)
            {
                var requestDto = this.mapper.Map<IncomingSubjectRequestDto>(item.request);

                if (item.subject != null) 
                {
                    var subjectInfoDto = this.mapper.Map<SubjectInfoDto>(item.subject);
                    requestDto.SubjectInfo = subjectInfoDto;
                }

                subjectRequestDtos.Add(requestDto);
            }

            return subjectRequestDtos;
        }

        public async Task<OneOf<List<OutgoingSubjectRequestDto>, NotFound>> GetAllBySubjectIdAsync(int subjectId)
        {
            var subject = await this.subjectRepository.GetById(subjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            var subjectRequests = this.subjectRequestRepository.GetAll().Where(x => x.SubjectId == subject.Id);
            var students = this.studentRepository.GetAll().Where(x => subjectRequests.Select(x => x.StudentId).Contains(x.Id)).ToList();

            var data = subjectRequests.ToList().Zip(students, (request, student) => new { request = request, student = student });

            var subjectRequestDtos = new List<OutgoingSubjectRequestDto>();

            foreach (var item in data)
            {
                OutgoingSubjectRequestDto requestDto = this.mapper.Map<OutgoingSubjectRequestDto>(item.request);

                if (item.student is not null)
                {
                    requestDto.StudentEmail = item.student.Email;
                }

                subjectRequestDtos.Add(requestDto);
            }

            return subjectRequestDtos;
        }

        public async Task<OneOf<Success, UserNotFound, UserAlreadyExists, NotFound>> JoinSubjectByRequestIdAsync(int subjectRequestId)
        {
            string email = this.currentUser.GetEmail();
            var student = await this.studentRepository.GetByEmail(email);

            if (student is null)
            {
                return this.currentUser.UserNotFound("Student");
            }

            var subjectRequest = await this.subjectRequestRepository.GetById(subjectRequestId);
            if (subjectRequest is null)
            {
                return new NotFound();
            }

            var subject = await this.subjectRepository.GetById(subjectRequest.SubjectId);
            if (subject is null)
            {
                return new NotFound();
            }

            if (subject.Students.Any(s => s.Id == student.Id))
            {
                return new UserAlreadyExists($"Student with email \"{email}\" is already registered to this subject.");
            }

            subject.Students.Add(student);
            await this.subjectRepository.Update(subject);
            await this.subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }

        public async Task<OneOf<Success>> RejectRequestByIdAsync(int subjectRequestId)
        {
            var subjectRequest = await this.subjectRequestRepository.GetById(subjectRequestId);
            if (subjectRequest is null)
            {
                return new Success();
            }

            await subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }

        public async Task<OneOf<Success>> DeleteByIdAsync(int subjectRequestId)
        {
            var subjectRequest = await this.subjectRequestRepository.GetById(subjectRequestId);
            if (subjectRequest is null)
            {
                return new Success();
            }

            await subjectRequestRepository.Delete(subjectRequest);

            return new Success();
        }
    }
}
