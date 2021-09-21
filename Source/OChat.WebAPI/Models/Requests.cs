using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OChat.WebAPI.Models
{
    public record AcceptFriendRequest(Guid UserId, Guid RequestId);

    public record CreateChat(Guid[] ParticipantsIds, String ChatName);

    public record Login(String Username, String Email, String Password);

    public record SendFriendRequest(Guid UserId, Guid TargetUserId);

    public record SendMessage(Guid SenderId, Guid ChatId, String Message);

    public record IgnoreFriendRequest(Guid UserId, Guid RequestId);

    public record RemoveFriend(Guid UserId, Guid FriendId);

    public record TimeLastMessageWasSeen(Guid UserId, Guid ChatId);
}
