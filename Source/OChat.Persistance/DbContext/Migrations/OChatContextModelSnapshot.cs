﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OChat.Infrastructure.Persistance;

namespace OChat.Persistance.DbContext.Migrations
{
    [DbContext(typeof(OChatContext))]
    partial class OChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ChatRoomUser", b =>
                {
                    b.Property<Guid>("ChatRoomsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ParticipantsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChatRoomsId", "ParticipantsId");

                    b.HasIndex("ParticipantsId");

                    b.ToTable("ChatRoomUser");
                });

            modelBuilder.Entity("OChat.Domain.ChatRoom", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("OChat.Domain.ChatTracker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChatId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastReadMessageTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatTrackers");
                });

            modelBuilder.Entity("OChat.Domain.Connection", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("OChat.Domain.FriendRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FromId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FromId");

                    b.ToTable("FriendRequests");
                });

            modelBuilder.Entity("OChat.Domain.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ChatRoomId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SentOn")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("OChat.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("UserFriends", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("FriendId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "FriendId");

                    b.HasIndex("FriendId");

                    b.ToTable("UserFriends");
                });

            modelBuilder.Entity("ChatRoomUser", b =>
                {
                    b.HasOne("OChat.Domain.ChatRoom", null)
                        .WithMany()
                        .HasForeignKey("ChatRoomsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OChat.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OChat.Domain.ChatTracker", b =>
                {
                    b.HasOne("OChat.Domain.ChatRoom", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId");

                    b.HasOne("OChat.Domain.User", null)
                        .WithMany("ChatTrackers")
                        .HasForeignKey("UserId");

                    b.Navigation("Chat");
                });

            modelBuilder.Entity("OChat.Domain.Connection", b =>
                {
                    b.HasOne("OChat.Domain.User", null)
                        .WithMany("Connections")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("OChat.Domain.FriendRequest", b =>
                {
                    b.HasOne("OChat.Domain.User", "From")
                        .WithMany("FriendRequests")
                        .HasForeignKey("FromId");

                    b.Navigation("From");
                });

            modelBuilder.Entity("OChat.Domain.Message", b =>
                {
                    b.HasOne("OChat.Domain.ChatRoom", null)
                        .WithMany("Messages")
                        .HasForeignKey("ChatRoomId");

                    b.HasOne("OChat.Domain.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("UserFriends", b =>
                {
                    b.HasOne("OChat.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("FriendId")
                        .HasConstraintName("FK_UserFriends_Friend_FriendId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OChat.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_UserFriends_User_UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("OChat.Domain.ChatRoom", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("OChat.Domain.User", b =>
                {
                    b.Navigation("ChatTrackers");

                    b.Navigation("Connections");

                    b.Navigation("FriendRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
