using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChat.Core.OChat.Services.InputModels
{
    public record SendFriendRequestModel(Guid UserId, Guid TargetUserId);

    public record AcceptFriendRequestModel(Guid UserId, Guid RequestId);

    public record IgnoreFriendRequestModel(Guid UserId, Guid RequestId);

    public record RemoveFriendModel(Guid UserId, Guid FriendId);

    public record CreateChatRoomModel(String ChatName, Guid[] ParticipantsIds);

    public record GetChatRoomMessageHistoryModel(Guid ChatId, Int32 Page, Int32 PageSize);

    public record GetTimeLastMessageWasSeenModel(Guid UserId, Guid ChatId);

    public record UpdateTimeLastMessageWasSeenModel(Guid UserId, Guid ChatId, DateTime TimeLastMessageWasSeen);

}
