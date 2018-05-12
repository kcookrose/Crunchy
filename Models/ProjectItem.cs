using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
    public class ProjectItem {

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
        public IList<StatusItem> ValidStatuses { get; set; }


        /// <summary>
        /// The set of users with visibility to the project. If empty,
        /// all users are assumed to have visibility.
        /// </summary>
        public IList<UserItem> OwnerUsers { get; set; }


        /// <summary>
        /// Tag meta-data. Multiple tags should be separated by semi-colons.
        /// </summary>
        public string Tags { get; set; }


        /// <summary>
        /// Files associated with the project.
        /// </summary>
        public IList<FileRef> Files { get; set; }


        public ProjectItem() {
            ValidStatuses = new List<StatusItem>();
            OwnerUsers = new List<UserItem>();
            Files = new List<FileRef>();
        }
        
    }
}