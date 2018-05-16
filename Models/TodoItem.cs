using System;
using System.ComponentModel.DataAnnotations;
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
        /// When part of a task group, this will be the owning task.
        /// </summary>
        public TodoItem OwnerTodoItem { get; set; }


        /// <summary>
        /// The project the task belongs to.
        /// </summary>
        [Required]
        public Project Project { get; set; }


        /// <summary>
        /// The current status of the task.
        /// </summary>
        [Required]
        public Status Status { get; set; }


        /// <summary>
        /// The user the task is assigned to.
        /// </summary>
        public User Assignee { get; set; }


        /// <summary>
        /// Todo items that must be completed before this one.
        /// </summary>
        public IList<TodoItem> RequiredItems{ get; set; }


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
            builder.Entity<TodoItem>().HasMany(ent => ent.Files).WithOne();
            builder.Entity<TodoItem>().HasOne(ent => ent.Project).WithMany();
        }
        
    }

}