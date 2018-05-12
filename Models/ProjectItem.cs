using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
    public class ProjectItem {
        [Key]
        public long Pid { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        // TODO status

        public List<UserItem> OwnerUsers { get; set; }

        //public List<string> Tags { get; set; }

        //public List<string> Files { get; set; }
        
    }
}