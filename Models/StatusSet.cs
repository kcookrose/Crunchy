using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

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


        /// <summary>
        /// Add a new status to the set.
        /// </summary>
        /// <param name="statusName">The name of the status to add</param>
        /// <param name="statusColor">The color of the status to add</param>
        public void Add(string statusName, string statusColor) {
            Statuses.Add(new Status(statusName, statusColor));
        }


        /// <summary>
        /// To string
        /// </summary>
        override public string ToString() {
            string res = base.ToString();
            res += '[';
            for (int i = 0; i < Statuses.Count; i++) {
                res += Statuses[i];
                if (i != Statuses.Count-1)
                    res += ", ";
            }
            res += ']';
            return res;
        }


        public static void OnModelCreating(TodoContext context, ModelBuilder modelBuilder) {
            modelBuilder.Entity<StatusSet>().HasMany(ss => ss.Statuses).WithOne();
        }
    }
}