using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models
{
    public class TodoItem {
        [Key]
        public long Tid { get; set; }
        
        public string Name { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime DueDateTime { get; set; }

        //public List<string> Tags { get; set; }

        public TimeSpan EstimatedTime { get; set; }

        public IList<FileRef> Files { get; set; } 

        public TodoItem OwnerTodoItem { get; set; }

        public ProjectItem OwnerProject { get; set; }

        // TODO Status
        
        public UserItem Assignee { get; set; }
        
    }

}