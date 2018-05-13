using System;
using System.ComponentModel.DataAnnotations;
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
    }
}