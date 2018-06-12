using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

namespace Crunchy.Models
{
    public class Status {

        /// <summary>
        /// Unique ID for the status.
        /// </summary>
        [Key]
        [JsonIgnore]
        public long Sid { get; set; }


        /// <summary>
        /// Name of the status.
        /// </summary>
        [Required]
        public string Name { get; set; }


        /// <summary>
        /// Color for task, as Hex/RGB
        /// i.e. #FF00FF for purple.
        /// </summary>
        public string Color { get; set; }

        public Status() { }

        public Status(string name) {
            Name = name;
        }

        public Status(string name, string color) {
            Name = name;
            Color = color;
        }


        /// <summary>
        /// To string
        /// </summary>
        override public string ToString() {
            string res = base.ToString();
            res += '[' + Name + ']';
            return res;
        }
    }

}