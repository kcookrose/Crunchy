using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {

    public class StatusSet {

        /// <summary>
        /// Unique key for the status set.
        /// </summary>
        public long Id { get; set; }


        /// <summary>
        /// The human-readable description of the status set.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The set of statues.
        /// </summary>
        public IList<Status> Statuses { get; set; }


        public StatusSet() {
            Statuses = new List<Status>();
        }


        public StatusSet(string name, params Status[] statuses) {
            Name = name;
            Statuses = new List<Status>(statuses);
        }


        /// <summary>
        /// Add a new status to the set.
        /// </summary>
        /// <param name="status">The status to add.</param>
        public void Add(Status status) {
            Statuses.Add(status);
        }


        /// <summary>
        /// Add a new status to the set.
        /// </summary>
        /// <param name="statusName">The name for the status to add</param>
        public void Add(string statusName) {
            Statuses.Add(new Status(statusName));
        }


        public static void OnModelCreating(TodoContext context, ModelBuilder modelBuilder) {
            modelBuilder.Entity<StatusSet>().HasMany(ss => ss.Statuses).WithOne();
        }
    }
}