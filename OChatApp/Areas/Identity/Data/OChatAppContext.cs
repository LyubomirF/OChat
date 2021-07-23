using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OChatApp.Areas.Identity.Data;

namespace OChatApp.Data
{
    public class OChatAppContext : IdentityDbContext<OChatAppUser>
    {
        public OChatAppContext(DbContextOptions<OChatAppContext> options)
            : base(options)
        {
        }

        public DbSet<ChatRoom> ChatRooms { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ChatRoom>()
                .HasKey(c => c.Id);

            builder.Entity<Message>()
                .HasKey(m => m.Id);

            builder.Entity<ChatRoom>()
                .HasMany<OChatAppUser>(c => c.Users)
                .WithMany(u => u.ChatRooms);

            builder.Entity<ChatRoom>()
                .HasMany<Message>(c => c.Messages)
                .WithOne(m => m.ChatRoom);

            builder.Entity<Message>()
                .HasOne<OChatAppUser>(m => m.From)
                .WithMany();
        }
    }
}
