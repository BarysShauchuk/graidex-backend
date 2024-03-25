using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Authentication.Login
{
    public class NewLoginNotification : INotification
    {
        public required string Role { get; set; }
        public required string Email { get; set; }
        public required NewLoginData Data { get; set; }
    }
}
