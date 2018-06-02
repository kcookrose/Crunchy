using System;
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
        /// A default project to assign tasks to.
        /// </summary>
        public Project DefaultProject { get; set; } 


        /// <summary>
        /// The id of a default project to assign tasks to.
        /// </summary>
        [NotMapped]
        public long DefaultProjectId {
            get {
                if (DefaultProject == null)
                    return -1;
                return DefaultProject.Pid;
            }
        }
    }
}