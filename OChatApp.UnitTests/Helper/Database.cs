using OChatApp.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OChatApp.UnitTests.Helper
{
    class Database
    {
        private static readonly List<OChatAppUser> _users = new()
        {
                new OChatAppUser()
                {
                    Id = "1",
                    UserName = "Richard",
                    Friends = new List<OChatAppUser>()
                    {
                        new OChatAppUser() { Id = "2", UserName = "Scot" },
                        new OChatAppUser() { Id = "3", UserName = "Greg" }
                    }
                },
                new OChatAppUser()
                {
                    Id = "2",
                    UserName = "Scot",
                    Friends = new List<OChatAppUser>()
                    {
                        new OChatAppUser() { Id = "1", UserName = "Richard" },
                        new OChatAppUser() { Id = "3", UserName = "Greg" }
                    }
                },
                new OChatAppUser()
                {
                    Id = "3",
                    UserName = "Greg",
                    Friends = new List<OChatAppUser>()
                    {
                        new OChatAppUser() { Id = "1", UserName = "Richard" },
                        new OChatAppUser() { Id = "2", UserName = "Scot" }
                    }
                },
                new OChatAppUser()
                {
                    Id = "4",
                    UserName = "John",
                    Friends = new List<OChatAppUser>(),
                },
                new OChatAppUser()
                {
                    Id = "5",
                    UserName = "Susan",
                    Friends = new List<OChatAppUser>(),
                },
            };

        private static readonly List<ChatRoom> _chats = new()
        {
            new ChatRoom()
            {
                Id = "1",
                Name = "Richard, Scot",
                Users = new List<OChatAppUser>()
                {
                    _users[0],
                    _users[1]
                }
            }
        };
        public Database()
        {
            //User service tests setup
            _users[2].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = "1",
                    Status = RequestStatus.Pending,
                    FromUser = _users[3]
                },
                new FriendRequest()
                {
                    Id = "2",
                    Status = RequestStatus.Pending,
                    FromUser = _users[4]
                }
            };
            _users[0].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = "0",
                    Status = RequestStatus.Accepted,
                    FromUser = _users[1]
                },
                new FriendRequest()
                {
                    Id = "3",
                    Status = RequestStatus.Pending,
                    FromUser = _users[3]
                },
                new FriendRequest()
                {
                    Id = "4",
                    Status = RequestStatus.Pending,
                    FromUser = _users[4]
                }
            };

            //Chat service tests setup
            _chats[0].Messages = new List<Message>
            {
                new Message()
                {
                    Id = "1",
                    From = _users[0],
                    Content = "Hello.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 30)
                },
                new Message()
                {
                    Id = "2",
                    From = _users[1],
                    Content = "Hi.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 40)
                },
                new Message()
                {
                    Id = "3",
                    From = _users[0],
                    Content = "What's up.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 0)
                },
                new Message()
                {
                    Id = "4",
                    From = _users[1],
                    Content = "Nothing much.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 20)
                }
            };
        }

        public List<OChatAppUser> Users => _users;

        public List<ChatRoom> Chats => _chats;

    }
}
