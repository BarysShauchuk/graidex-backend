using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graidex.Application.Notifications.Authentication.Login
{
    public class NewLoginData
    {
        public string? IpAddress { get; set; }
        public DateTimeOffset LoginTime { get; set; }
    }
}
