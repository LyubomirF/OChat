using OChat.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests.Helper
{
    class Database
    {
        public static List<User> Users { get; private set; } = new()
        {
            new User()
            {
                Id = Guid.Parse("913a77a8-ec51-444a-8e46-f0ee02feff19"),
                Username = "Richard",
                Friends = new List<User>()
            },
            new User()
            {
                Id = Guid.Parse("c4152c12-bb30-46c3-ac0c-c18864dd958d"),
                Username = "Scot",
                Friends = new List<User>()
            },
            new User()
            {
                Id = Guid.Parse("abc26479-3bd9-4ee9-8c7b-62277703611c"),
                Username = "Greg",
                Friends = new List<User>()
            },
            new User()
            {
                Id = Guid.Parse("0abde8d1-5f27-4c00-87a0-61ce84c66afe"),
                Username = "John",
                Friends = new List<User>(),
            },
            new User()
            {
                Id = Guid.Parse("ec82b73c-50ea-4313-85ab-3f13849e72f5"),
                Username = "Susan",
                Friends = new List<User>(),
            },
        };

        private static List<ChatRoom> Chats = new()
        {
            new ChatRoom()
            {
                Id = Guid.Parse("b2090fc4-be44-4f31-b50a-45cd1b2ce4c9"),
                Name = "Richard, Scot",
                Participants = new List<User>()
                {
                    Users[0],
                    Users[1]
                }
            }
        };

        static Database()
        {
            Users[0].Friends.Add(Users[1]);
            Users[0].Friends.Add(Users[2]);

            Users[1].Friends.Add(Users[0]);
            Users[1].Friends.Add(Users[1]);

            Users[2].Friends.Add(Users[0]);
            Users[2].Friends.Add(Users[1]);

            //User service tests setup
            Users[2].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = Guid.Parse("61dde34e-791a-4ff5-a4b0-19b4468de008"),
                    Status = FriendRequestStatus.Pending,
                    From = Users[3]
                },
                new FriendRequest()
                {
                    Id = Guid.Parse("0a60f994-3dd8-4606-9b11-8946e6f9b5e7"),
                    Status = FriendRequestStatus.Pending,
                    From = Users[4]
                }
            };
            Users[0].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = Guid.Parse("db7415eb-2574-4884-9fc9-f8effa7848db"),
                    Status = FriendRequestStatus.Accepted,
                    From = Users[1]
                },
                new FriendRequest()
                {
                    Id = Guid.Parse("4e8da9bf-08fe-4654-9089-f3431dc66081"),
                    Status = FriendRequestStatus.Pending,
                    From = Users[3]
                },
                new FriendRequest()
                {
                    Id = Guid.Parse("a23a0265-00fb-4339-b04a-7ebeb59a7556"),
                    Status = FriendRequestStatus.Pending,
                    From = Users[4]
                }
            };

            //Chat service tests setup
            Chats[0].Messages = new List<Message>
            {
                new Message()
                {
                    Id = Guid.Parse("6b8317dc-a081-4b8d-9eb6-f80746372a94"),
                    Sender = Users[0],
                    Content = "Hello.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 30)
                },
                new Message()
                {
                    Id = Guid.Parse("bb2405e6-1628-4c10-8d4a-ec71bec1a986"),
                    Sender = Users[1],
                    Content = "Hi.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 40)
                },
                new Message()
                {
                    Id = Guid.Parse("83b388e1-244d-4412-bd48-cedbe6c1df1a"),
                    Sender = Users[0],
                    Content = "What's up.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 0)
                },
                new Message()
                {
                    Id = Guid.Parse("d5fe4715-bcfd-47d4-8086-756ca83088bb"),
                    Sender = Users[1],
                    Content = "Nothing much.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 20)
                }
            };
        }
    }
}
