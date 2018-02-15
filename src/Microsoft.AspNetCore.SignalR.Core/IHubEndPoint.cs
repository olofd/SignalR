using System.Threading.Tasks;
using Microsoft.AspNetCore.Protocols;

namespace Microsoft.AspNetCore.SignalR
{
    public interface IHubEndPoint
    {
        Task OnConnectedAsync(ConnectionContext connection);
    }
}