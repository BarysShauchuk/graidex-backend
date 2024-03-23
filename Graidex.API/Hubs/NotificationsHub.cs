using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Graidex.API.Hubs
{
    [Authorize]
    public class NotificationsHub : Hub<INotificationsClient>
    {
        public override async Task OnConnectedAsync()
        {
            // TODO: Notify about new login

            await base.OnConnectedAsync();
        }

        public async Task CallMethodOnServer()
        {
            await Task.Delay(1);
            Console.WriteLine("CallMethodOnServer");
        }
    }
}
