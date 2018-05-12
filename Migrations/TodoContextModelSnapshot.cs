﻿// <auto-generated />
using Crunchy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Crunchy.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Crunchy.Models.FileRef", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ProjectItemPid");

                    b.Property<string>("RepoUrl");

                    b.Property<long?>("TodoItemTid");

                    b.HasKey("Id");

                    b.HasIndex("ProjectItemPid");

                    b.HasIndex("TodoItemTid");

                    b.ToTable("FileRef");
                });

            modelBuilder.Entity("Crunchy.Models.ProjectItem", b =>
                {
                    b.Property<long>("Pid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<string>("Tags");

                    b.HasKey("Pid");

                    b.ToTable("ProjectItems");
                });

            modelBuilder.Entity("Crunchy.Models.StatusItem", b =>
                {
                    b.Property<long>("Sid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<string>("Name");

                    b.Property<long?>("ProjectItemPid");

                    b.HasKey("Sid");

                    b.HasIndex("ProjectItemPid");

                    b.ToTable("StatusItems");
                });

            modelBuilder.Entity("Crunchy.Models.TodoItem", b =>
                {
                    b.Property<long>("Tid")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AssigneeUid");

                    b.Property<DateTime>("DueDateTime");

                    b.Property<TimeSpan>("EstimatedTime");

                    b.Property<string>("Name");

                    b.Property<long?>("OwnerProjectPid");

                    b.Property<long?>("OwnerTodoItemTid");

                    b.Property<DateTime>("StartDateTime");

                    b.HasKey("Tid");

                    b.HasIndex("AssigneeUid");

                    b.HasIndex("OwnerProjectPid");

                    b.HasIndex("OwnerTodoItemTid");

                    b.ToTable("TodoItems");
                });

            modelBuilder.Entity("Crunchy.Models.UserItem", b =>
                {
                    b.Property<long>("Uid")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<long?>("ProjectItemPid");

                    b.HasKey("Uid");

                    b.HasIndex("ProjectItemPid");

                    b.ToTable("UserItems");
                });

            modelBuilder.Entity("Crunchy.Models.FileRef", b =>
                {
                    b.HasOne("Crunchy.Models.ProjectItem")
                        .WithMany("Files")
                        .HasForeignKey("ProjectItemPid");

                    b.HasOne("Crunchy.Models.TodoItem")
                        .WithMany("Files")
                        .HasForeignKey("TodoItemTid");
                });

            modelBuilder.Entity("Crunchy.Models.StatusItem", b =>
                {
                    b.HasOne("Crunchy.Models.ProjectItem")
                        .WithMany("ValidStatuses")
                        .HasForeignKey("ProjectItemPid");
                });

            modelBuilder.Entity("Crunchy.Models.TodoItem", b =>
                {
                    b.HasOne("Crunchy.Models.UserItem", "Assignee")
                        .WithMany()
                        .HasForeignKey("AssigneeUid");

                    b.HasOne("Crunchy.Models.ProjectItem", "OwnerProject")
                        .WithMany()
                        .HasForeignKey("OwnerProjectPid");

                    b.HasOne("Crunchy.Models.TodoItem", "OwnerTodoItem")
                        .WithMany()
                        .HasForeignKey("OwnerTodoItemTid");
                });

            modelBuilder.Entity("Crunchy.Models.UserItem", b =>
                {
                    b.HasOne("Crunchy.Models.ProjectItem")
                        .WithMany("OwnerUsers")
                        .HasForeignKey("ProjectItemPid");
                });
#pragma warning restore 612, 618
        }
    }
}
