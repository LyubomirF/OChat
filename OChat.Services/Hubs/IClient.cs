using System.Threading.Tasks;

namespace OChat.Services.Hubs
{
    public interface IClient
    {
        Task ReceiveMessage(string message);
    }
}
