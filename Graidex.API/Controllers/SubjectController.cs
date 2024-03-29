﻿using Graidex.Application.DTOs.Subject;
using Graidex.Application.Services.Subjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Graidex.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            this.subjectService = subjectService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Teacher")]
        public async Task<ActionResult<SubjectDto>> Create(CreateSubjectDto request)
        {
            var result = await this.subjectService.CreateForCurrentAsync(request);

            return result.Match<ActionResult<SubjectDto>>(
                subjectDto => Ok(subjectDto),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("all")]
        [Authorize(Roles = "Student, Teacher")]
        public async Task<ActionResult> GetAll()
        {
            var result = await this.subjectService.GetAllOfCurrentAsync();

            return result.Match<ActionResult>(
                subjectDtos => Ok(subjectDtos),
                userNotFound => NotFound(userNotFound.Comment));
        }

        [HttpGet("of-teacher/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> GetSubjectOfTeacherById(int subjectId)
        {
            var result = await this.subjectService.GetSubjectOfTeacherByIdAsync(subjectId);

            return result.Match<ActionResult>(
                subjectInfoDto => Ok(subjectInfoDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpGet("of-student/{subjectId}")]
        [Authorize(Roles = "Student", Policy = "StudentOfSubject")]
        public async Task<ActionResult> GetSubjectOfStudentById(int subjectId)
        {
            var result = await this.subjectService.GetSubjectOfStudentByIdAsync(subjectId);

            return result.Match<ActionResult>(
                subjectInfoDto => Ok(subjectInfoDto),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpPut("update/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> Update(int subjectId, UpdateSubjectDto updateSubjectDto)
        {
            var result = await this.subjectService.UpdateSubjectInfoAsync(subjectId, updateSubjectDto);

            return result.Match<ActionResult>(
                success => Ok(),
                validationFailed => BadRequest(validationFailed.Errors),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }

        [HttpDelete("delete/{subjectId}")]
        [Authorize(Roles = "Teacher", Policy = "TeacherOfSubject")]
        public async Task<ActionResult> Delete(int subjectId)
        {
            var result = await this.subjectService.DeleteByIdAsync(subjectId);

            return result.Match<ActionResult>(
                success => Ok(),
                userNotFound => NotFound(userNotFound.Comment),
                notFound => NotFound());
        }
    }
}
