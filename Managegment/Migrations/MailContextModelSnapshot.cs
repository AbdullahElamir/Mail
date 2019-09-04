﻿// <auto-generated />
using Managegment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;

namespace Managegment.Migrations
{
    [DbContext(typeof(MailContext))]
    partial class MailContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Managegment.Models.ActorBase", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("Email");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("Actors");

                    b.HasDiscriminator<string>("Discriminator").HasValue("ActorBase");
                });

            modelBuilder.Entity("Managegment.Models.Conversation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEmailEnabled");

                    b.Property<bool>("IsSMSEnabled");

                    b.Property<string>("Subject");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("Id");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("Managegment.Models.GroupMember", b =>
                {
                    b.Property<long>("GroupMemberId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("GroupId");

                    b.Property<long?>("MemberId");

                    b.HasKey("GroupMemberId");

                    b.HasIndex("GroupId");

                    b.HasIndex("MemberId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("Managegment.Models.Message", b =>
                {
                    b.Property<long>("MessageId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorId");

                    b.Property<long>("ConversationId");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Payload");

                    b.HasKey("MessageId");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ConversationId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("Managegment.Models.Participation", b =>
                {
                    b.Property<long>("ConversationId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ConversationId1");

                    b.Property<long>("MemberId");

                    b.HasKey("ConversationId");

                    b.HasIndex("ConversationId1");

                    b.HasIndex("MemberId");

                    b.ToTable("Participations");
                });

            modelBuilder.Entity("Managegment.Models.Transaction", b =>
                {
                    b.Property<long>("TransactionId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("ConvId");

                    b.Property<bool>("IsRead");

                    b.Property<long>("MemberId");

                    b.Property<long>("MessageId");

                    b.Property<DateTime>("TimeStamp");

                    b.HasKey("TransactionId");

                    b.HasIndex("MemberId");

                    b.HasIndex("MessageId");

                    b.ToTable("MessageTransactions");
                });

            modelBuilder.Entity("Managegment.Models.Group", b =>
                {
                    b.HasBaseType("Managegment.Models.ActorBase");


                    b.ToTable("Group");

                    b.HasDiscriminator().HasValue("Group");
                });

            modelBuilder.Entity("Managegment.Models.User", b =>
                {
                    b.HasBaseType("Managegment.Models.ActorBase");

                    b.Property<DateTime?>("BirthDate");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime>("CreatedOn");

                    b.Property<short?>("Gender");

                    b.Property<long?>("ModifiedBy");

                    b.Property<DateTime?>("ModifiedOn");

                    b.Property<string>("Password");

                    b.Property<short?>("Status");

                    b.ToTable("User");

                    b.HasDiscriminator().HasValue("User");
                });

            modelBuilder.Entity("Managegment.Models.GroupMember", b =>
                {
                    b.HasOne("Managegment.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId");

                    b.HasOne("Managegment.Models.ActorBase", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId");
                });

            modelBuilder.Entity("Managegment.Models.Message", b =>
                {
                    b.HasOne("Managegment.Models.ActorBase", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Managegment.Models.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Managegment.Models.Participation", b =>
                {
                    b.HasOne("Managegment.Models.Conversation", "Conversation")
                        .WithMany()
                        .HasForeignKey("ConversationId1");

                    b.HasOne("Managegment.Models.ActorBase", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Managegment.Models.Transaction", b =>
                {
                    b.HasOne("Managegment.Models.ActorBase", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Managegment.Models.Message", "Message")
                        .WithMany()
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
