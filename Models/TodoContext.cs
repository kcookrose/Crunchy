using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crunchy.Models {

    public class TodoContext: DbContext {
        
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<StatusSet> StatusSets { get; set; }

        public DbSet<Status> Statuses { get; set; }
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=crunchy.db");    
        }

        protected override void OnModelCreating(ModelBuilder builder) {
            TodoItem.OnModelCreating(this, builder);
            FileRef.OnModelCreating(this, builder);
            StatusSet.OnModelCreating(this, builder);
        }


        public void EnsureDeepLoaded(object value) {
            var entry = Entry(value);
            if (entry.State == EntityState.Detached)
                entry = Attach(value);
            EnsureDeepLoaded(entry);
        }


        public void EnsureDeepLoaded(EntityEntry entry) {
            foreach (var collection in entry.Collections) {
                collection.Load();
            }
            foreach (var navigation in entry.Navigations) {
                navigation.Load();
            }
        }
    }


}