using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

// TODO: Add Project/Owneruser join table

namespace Crunchy.Models {
    public class Project {

        /// <summary>
        /// Unique ID for the project.
        /// </summary>
        [Key]
        public long Pid { get; set; }


        /// <summary>
        /// Short description of the project.
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Long description of the project.
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// The set of statuses that may be applied to todo items assigned to
        /// this project. If empty, all statuses should be assumed valid.
        /// </summary>
        public StatusSet ValidStatuses { get; set; }


        /// <summary>
        /// The set of users with visibility to the project. If empty,
        /// all users are assumed to have visibility.
        /// </summary>
        public IList<User> OwnerUsers { get; set; }


        /// <summary>
        /// Tag meta-data. Multiple tags should be separated by semi-colons.
        /// </summary>
        public string Tags { get; set; }


        /// <summary>
        /// Files associated with the project.
        /// </summary>
        public IList<FileRef> Files { get; set; }


        public Project() {
            OwnerUsers = new List<User>();
            Files = new List<FileRef>();
        }

        public static void OnModelCreating(TodoContext context, ModelBuilder builder) {
            builder.Entity<Project>().HasMany(prj => prj.Files).WithOne();
        }
        
    }
}