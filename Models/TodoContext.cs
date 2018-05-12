using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {

    public class TodoContext: DbContext {
        
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<UserItem> UserItems { get; set; }

        public DbSet<StatusItem> StatusItems { get; set; }

        public DbSet<ProjectItem> ProjectItems { get; set; }
 
        // Constructor 
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=crunchy.db");    
        }
    }


}