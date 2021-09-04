using System.Threading.Tasks;

namespace OChat.Infrastructure.Hubs
{
    public interface IClient
    {
        Task ReceiveMessage(string message);
    }
}
