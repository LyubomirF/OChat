using OChatApp.Areas.Identity.Data;
using System.Threading.Tasks;

namespace OChatApp.Repositories
{
    public interface IChatRepository : IRepository<ChatRoom>
    {
        Task<ChatRoom> GetChatRoomWithMessegesAsync(string chatId, int page, int pageSize);

        Task<ChatRoom> GetChatWithUsersAsync(string chatId);
    }
}
