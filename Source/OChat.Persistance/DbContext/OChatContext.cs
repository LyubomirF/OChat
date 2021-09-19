using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OChat.Domain;

namespace OChat.Infrastructure.Persistance
{
    public class OChatContext : DbContext
    {
        public OChatContext(DbContextOptions<OChatContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<ChatTracker> ChatTrackers { get; set; }

        public DbSet<Connection> Connections { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasKey(u => u.Id);

            builder.Entity<User>()
                .HasMany<User>(u => u.Friends)
                .WithMany(f => f.Friends)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFriends",
                        x => x.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("UserId")
                            .HasConstraintName("FK_UserFriends_User_UserId")
                            .OnDelete(DeleteBehavior.NoAction),
                        x => x.HasOne<User>()
                            .WithMany()
                            .HasForeignKey("FriendId")
                            .HasConstraintName("FK_UserFriends_Friend_FriendId")
                            .OnDelete(DeleteBehavior.Cascade),
                        x => x.HasKey(new string[] { "UserId", "FriendId" }));

            builder.Entity<User>()
                .HasMany<FriendRequest>(u => u.FriendRequests)
                .WithOne(r => r.From);

            builder.Entity<User>()
                .HasMany<Connection>(u => u.Connections);

            builder.Entity<User>()
                .HasMany<ChatTracker>(u => u.ChatTrackers);

            builder.Entity<ChatRoom>()
                .HasKey(c => c.Id);

            builder.Entity<ChatRoom>()
                .HasMany<User>(c => c.Participants)
                .WithMany("ChatRooms");

            builder.Entity<ChatRoom>()
                .HasMany<Message>(c => c.Messages)
                .WithOne();

            builder.Entity<Message>()
                .HasKey(m => m.Id);

            builder.Entity<Message>()
                .HasOne<User>(m => m.Sender);

            builder.Entity<FriendRequest>()
                .HasOne<User>(fr => fr.From);

            builder.Entity<Connection>()
                .HasKey(c => c.Id);

            builder.Entity<ChatTracker>()
                .HasKey(x => x.Id);

            builder.Entity<ChatTracker>()
                .HasOne(ct => ct.Chat);
        }
    }
}
