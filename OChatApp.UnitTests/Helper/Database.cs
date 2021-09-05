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
        private static readonly List<User> _users = new()
        {
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "Richard",
                    Friends = new List<User>()
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "Scot",
                    Friends = new List<User>()
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "Greg",
                    Friends = new List<User>()
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "John",
                    Friends = new List<User>(),
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "Susan",
                    Friends = new List<User>(),
                },
            };

        private static readonly List<ChatRoom> _chats = new()
        {
            new ChatRoom()
            {
                Id = Guid.NewGuid(),
                Name = "Richard, Scot",
                Participants = new List<User>()
                {
                    _users[0],
                    _users[1]
                }
            }
        };
        public Database()
        {
            _users[0].Friends.Add(_users[1]);
            _users[0].Friends.Add(_users[2]);

            _users[1].Friends.Add(_users[0]);
            _users[1].Friends.Add(_users[1]);

            _users[2].Friends.Add(_users[0]);
            _users[2].Friends.Add(_users[1]);

            //User service tests setup
            _users[2].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = Guid.NewGuid(),
                    Status = FriendRequestStatus.Pending,
                    From = _users[3]
                },
                new FriendRequest()
                {
                    Id = Guid.NewGuid(),
                    Status = FriendRequestStatus.Pending,
                    From = _users[4]
                }
            };
            _users[0].FriendRequests = new List<FriendRequest>
            {
                new FriendRequest()
                {
                    Id = Guid.NewGuid(),
                    Status = FriendRequestStatus.Accepted,
                    From = _users[1]
                },
                new FriendRequest()
                {
                    Id = Guid.NewGuid(),
                    Status = FriendRequestStatus.Pending,
                    From = _users[3]
                },
                new FriendRequest()
                {
                    Id = Guid.NewGuid(),
                    Status = FriendRequestStatus.Pending,
                    From = _users[4]
                }
            };

            //Chat service tests setup
            _chats[0].Messages = new List<Message>
            {
                new Message()
                {
                    Id = Guid.NewGuid(),
                    Sender = _users[0],
                    Content = "Hello.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 30)
                },
                new Message()
                {
                    Id = Guid.NewGuid(),
                    Sender = _users[1],
                    Content = "Hi.",
                    SentOn = new DateTime(2021, 1, 1, 13, 15, 40)
                },
                new Message()
                {
                    Id = Guid.NewGuid(),
                    Sender = _users[0],
                    Content = "What's up.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 0)
                },
                new Message()
                {
                    Id = Guid.NewGuid(),
                    Sender = _users[1],
                    Content = "Nothing much.",
                    SentOn = new DateTime(2021, 1, 1, 13, 16, 20)
                }
            };
        }

        public List<User> Users => _users;

        public List<ChatRoom> Chats => _chats;

    }
}
