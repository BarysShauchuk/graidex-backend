using AutoMapper;
using FluentValidation;
using Graidex.Application.DTOs.Users;
using Graidex.Application.OneOfCustomTypes;
using Graidex.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Services.Users
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GetEmail()
        {
            var user = this.httpContextAccessor.HttpContext.User;
            var identity = user.Identity;
            if (identity is null)
            {
                throw new HttpRequestException();
            }

            var email = identity.Name;
            if (email is null)
            {
                throw new HttpRequestException();
            }

            return email;
        }

        public UserNotFound UserNotFound(string role = "User")
        {
            string email = this.GetEmail();
            return new UserNotFound($"{role} with email \"{email}\" is not found.");
        }

        public string GetRole()
        {
            if (this.httpContextAccessor.HttpContext.User.IsInRole("Student"))
            {
                return "Student";
            }

            if (this.httpContextAccessor.HttpContext.User.IsInRole("Teacher"))
            {
                return "Teacher";
            }

            else
            {
                return String.Empty;
            }
        }
    }
}
