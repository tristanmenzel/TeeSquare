using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TeeSquare.DemoApi.Hubs
{
    public class ApplicationHub: Hub<IApplicationHubClient>, IApplicationHubServer
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.Others.ReceiveMessage(user, message);
        }
    }

    public interface IApplicationHubServer
    {
        Task SendMessage(string user, string message);
    }

    public interface IApplicationHubClient
    {

        Task ReceiveMessage(string user, string message);
    }
}
