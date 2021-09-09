using OChatApp.UnitTests.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests
{
    class TestInputs
    {
        public static Guid[] GetInputFor_GetUserFriends_ReturnsCollectionOfFriends()
            => new Guid[] { Database.Users[0].Id };

        public static Guid[] GetInputFor_GetUserFriends_ThrowsNotFoundException()
            => new Guid[] { Guid.Parse("d6a8ce83-d5d1-4493-b729-2538ecffa33b") };

        public static Guid[] GetInputFor_GetUserFriends_ThrowsEmptyCollectionException()
            => new Guid[] { Database.Users[4].Id };

        public static IEnumerable<Guid[]> GetInputFor_SendFriendRequest_ValidCall()
            => new List<Guid[]> { new Guid[] { Database.Users[3].Id, Database.Users[1].Id } };

        public static IEnumerable<Guid[]> GetInputFor_AcceptFriendRequest_ValidCall()
            => new List<Guid[]> { new Guid[] { Database.Users[2].Id, Database.Users[2].FriendRequests.First().Id, Database.Users[3].Id } };

        public static IEnumerable<Guid[]> GetIntputFor_AcceptFriendRequest_InvalidRequest_ThrowsNotFoundException()
            => new List<Guid[]> { new Guid[] { Database.Users[2].Id, Guid.Parse("68f458fc-db04-484b-b1b1-f8502dc4a759"), Database.Users[3].Id } };

        public static IEnumerable<Guid[]> GetInputFor_AcceptFriendRequest_InvalidRequest_ThrowsFriendRequestException()
            => new List<Guid[]> { new Guid[] { Database.Users[0].Id, Database.Users[0].FriendRequests.FirstOrDefault().Id, Database.Users[1].Id } };

        public static IEnumerable<Guid[]> GetInputFor_IgnoreFriendRequest_ValidCall() 
            => new List<Guid[]> { new Guid[] { Database.Users[2].Id, Database.Users[2].FriendRequests.Skip(1).Take(1).SingleOrDefault().Id } };

        public static IEnumerable<Guid[]> GetInputFor_IgnoreFriendRequest_InvalidRequest_ThrowsNotFoundException()
            => new List<Guid[]> { new Guid[] { Database.Users[2].Id, Guid.Parse("b9c4a4a5-ef2f-4f83-b6ca-1f5680c99503") } };

        public static IEnumerable<Guid[]> GetInputFor_IgnoreFriendRequest_InvalidRequest_ThrowsFriendRequestException() 
            => new List<Guid[]> { new Guid[] { Database.Users[0].Id, Database.Users[0].FriendRequests.FirstOrDefault().Id } };

        public static IEnumerable<Guid[]> GetInputFor_RemoveFriend_ValidCall() 
            => new List<Guid[]> { new Guid[] { Database.Users[0].Id, Database.Users[1].Id } };

        public static Guid[] GetInputFor_GetPendingRequests_ValidCall()
            => new Guid[] { Database.Users[2].Id };
    }
}
