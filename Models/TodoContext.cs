using Microsoft.EntityFrameworkCore;

namespace Crunchy.Models {

    public class TodoContext: DbContext {
        
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<Project> Projects { get; set; }

//        public DbSet<FileRef> Files { get; set; }
 
        // Constructor 
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=crunchy.db");    
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            TodoItem.OnModelCreating(this, builder);
            FileRef.OnModelCreating(this, builder);
        }
    }


}