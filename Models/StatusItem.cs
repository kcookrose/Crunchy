using System;
using System.ComponentModel.DataAnnotations;
//using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models
{
    public class StatusItem {
        [Key]
        public long Sid { get; set; }

        public string Name { get; set; }
    }

}