using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
    public class TodoItem {

        /// <summary>
        /// A unique ID for the TodoItem.
        /// </summary>
        [Key]
        public long Tid { get; set; }


        /// <summary>
        /// The short description of the item.
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// The earliest time the item should be begun.
        /// </summary>
        public DateTime? StartDateTime { get; set; }


        /// <summary>
        /// The latest time the task should be completed.
        /// </summary>
        public DateTime? DueDateTime { get; set; }


        /// <summary>
        /// Tag meta-data. Multiple tags should be separated by semi-colons.
        /// </summary>
        public string Tags { get; set; }


        /// <summary>
        /// The estimated time required to complete the task.
        /// </summary>
        public TimeSpan? EstimatedTime { get; set; }


        /// <summary>
        /// Files associated with the task.
        /// </summary>
        public IList<FileRef> Files { get; set; }


        /// <summary>
        /// IDs of files associated with the item.
        /// </summary>
        [NotMapped]
        public long[] FileIds {
            get {
                if (Files != null)
                    return Files.Select(fileRef => fileRef.Id).ToArray();
                return new long[0];
            }
        }


        /// <summary>
        /// When part of a task group, this will be the owning task.
        /// </summary>
        [ForeignKey("OwnerTodoItemId")]
        public TodoItem OwnerTodoItem { get; set; }


        /// <summary>
        /// The ID of the owning todo item, of -1 if none.
        /// </summary>
        public long? OwnerTodoItemId { get; set; }


        /// <summary>
        /// The project the task belongs to.
        /// </summary>
        [Required]
        [ForeignKey("ProjectId")]
        public Project Project { get; set; }


        /// <summary>
        /// The ID of the project the item belongs to.
        /// </summary>
        public long ProjectId { get; set; }


        /// <summary>
        /// The current status of the task.
        /// </summary>
        [Required]
        [ForeignKey("StatusId")]
        public Status Status { get; set; }


        /// <summary>
        /// The ID of the current status
        /// </summary>
        public long StatusId { get; set; }


        /// <summary>
        /// The user the task is assigned to.
        /// </summary>
        [ForeignKey("AssigneeId")]
        public User Assignee { get; set; }


        /// <summary>
        /// The ID of the assignee or -1 if unassigned
        /// </summary>
        public long? AssigneeId { get; set; }


        /// <summary>
        /// Todo items that must be completed before this one.
        /// </summary>
        public IList<TodoItem> RequiredItems{ get; set; }


        /// <summary>
        /// The IDs of items that must be completed before this one.
        /// </summary>
        [NotMapped]
        public long[] RequiredItemIds {
            get {
                if (RequiredItems == null)
                    return new long[0];
                return RequiredItems
                    .Select(todoItem => todoItem.Tid)
                    .ToArray();
            }
        }


        public TodoItem() { }


        public TodoItem(Project project, Status status) {
            Files = new List<FileRef>();
            Project = project;
            Status = status;
        }


        /// <summary>
        /// Called when creating model for the TodoItem type.
        /// </summary>
        /// <param name="context">The context that will control the model</param>
        /// <param name="builder">The model builder.</param>
        public static void OnModelCreating(TodoContext context,
                ModelBuilder builder) {
            var entity = builder.Entity<TodoItem>();
            entity.HasMany(ent => ent.Files)
                .WithOne();
            entity.HasOne(ent => ent.Project)
                .WithMany()
                .HasForeignKey(todo => todo.ProjectId);

        }
        
    }

}