using System;
using OChat.Domain;

namespace OChat.Infrastructure.Identity
{
    public class ApplicationUserToDomainUserMapping
    {
        public string ApplicationUserID { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public Guid DomainUserId { get; set; }

        public User DomainUser { get; set; }
    }
}
