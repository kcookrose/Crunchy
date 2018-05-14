using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
//    [Owned]
    public class FileRef {

        /// <summary>
        /// Unique ID for the file ref.
        /// </summary>
        [Key]
        public long Id { get; set; }


        /// <summary>
        /// Url to be used in GET request to retrive file.
        /// </summary>
        [Required]
        public string RepoUrl { get; set; }


        public FileRef() { }


        public FileRef(string path) {
            RepoUrl = path;
        }


        public static void OnModelCreating(TodoContext context,
                ModelBuilder builder) {
            // Nada.
        }
    }
}