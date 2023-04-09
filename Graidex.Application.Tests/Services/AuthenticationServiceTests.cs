using Graidex.Application.Services.Authentication;
using Graidex.Application.Tests.Fakes;
using Graidex.Domain.Interfaces;
using Graidex.Domain.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Tests.Services
{
    internal class AuthenticationServiceTests
    {
        private IStudentRepository studentRepository;
        private ITeacherRepository teacherRepository;
        private IAuthenticationService authenticationService;

        [SetUp]
        public void Setup()
        {
            this.studentRepository = new FakeStudentRepository();
            this.teacherRepository = new FakeTeacherRepository();
            
            this.authenticationService =
                new AuthenticationService(this.studentRepository, this.teacherRepository);
        }
    }
}
