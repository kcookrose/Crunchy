using System;
using System.ComponentModel.DataAnnotations;
//using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {
    public class UserItem {
        [Key]
        public long Uid { get; set; }

        public string Name { get; set; }

        // TODO Default project 
    }
}