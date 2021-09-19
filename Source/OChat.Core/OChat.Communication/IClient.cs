using System.Threading.Tasks;

namespace OChat.Core.Communication
{
    public interface IClient
    {
        Task ReceiveMessage(string message);
    }
}
