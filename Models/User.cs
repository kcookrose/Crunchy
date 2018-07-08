using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
    public class User {

        /// <summary>
        /// A unique ID for the user.
        /// </summary>
        [Key]
        public long Uid { get; set; }


        /// <summary>
        /// User name.
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// A list of projects the user owns.
        /// </summary>
        public IList<ProjectOwner> OwnedProjects { get; set; }


        /// <summary>
        /// A default project to assign tasks to.
        /// </summary>
        [ForeignKey("DefaultProjectId")]
        public Project DefaultProject { get; set; } 


        /// <summary>
        /// The id of a default project to assign tasks to.
        /// </summary>
        public long? DefaultProjectId { get; set; }

        public static void OnModelCreating(TodoContext context, ModelBuilder builder) {
            builder.Entity<User>()
                .HasOne(user => user.DefaultProject)
                .WithMany()
                .HasForeignKey(user => user.DefaultProjectId);
        }
    }
}