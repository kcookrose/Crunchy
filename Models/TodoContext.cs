using System.Linq;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crunchy.Models {

    public class TodoContext: DbContext {
        
        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<StatusSet> StatusSets { get; set; }

        public DbSet<Status> Statuses { get; set; }

        public DbSet<FileRef> FileRefs { get; set; }
        
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlite("Data Source=crunchy.db");    
        }


        protected override void OnModelCreating(ModelBuilder builder) {
            User.OnModelCreating(this, builder);
            TodoItem.OnModelCreating(this, builder);
            FileRef.OnModelCreating(this, builder);
            StatusSet.OnModelCreating(this, builder);
            ProjectOwner.OnModelCreating(this, builder);
        }


        public void EnsureDeepLoaded(EntityEntry entry) {
            foreach (var navigation in entry.Navigations) {
                navigation.Load();
            }
        }


        public void DevSeedDatabase() {
            if (Statuses.Count() == 0)
                DevSeedStatuses();
            if (Users.Count() == 0)
                DevSeedUser();
            if (Projects.Count() == 0)
                DevSeedProject();
            if (TodoItems.Count() == 0)
                DevSeedTodoItem();
        }

        /// <summary>
        /// [Dev] Seed the Status table with statuses
        /// </summary>
        public void DevSeedStatuses() {
            var newStatusSet = new StatusSet("Test Status Set");

            newStatusSet.Add("TODO", "FF0000");
            newStatusSet.Add("In Progress", "FFFF00");
            newStatusSet.Add("Complete", "00FF00");
               
            StatusSets.Add(newStatusSet);
            SaveChanges();

            foreach (var ss in StatusSets) {
                EnsureDeepLoaded(Entry(ss));
                System.Console.WriteLine("---" + ss.ToString());
            }
        }


        /// <summary>
        /// [Dev] Seed the Todo table with an item.
        /// </summary>
        public void DevSeedTodoItem() {
            var newFile = new FileRef("test/file.jpg");

            var user = Users.FirstOrDefault();
            var statusSet = StatusSets.FirstOrDefault();

            var project = Projects.FirstOrDefault();
            if (project == null) {
                System.Console.WriteLine(" !!! Tried to create Todo without project");
                return;
            }
            EnsureDeepLoaded(Entry(project));
            EnsureDeepLoaded(Entry(project.ValidStatuses));
            var initItem = new TodoItem(project, project.ValidStatuses.Statuses[0]);
            initItem.Name = "Init Item";
            initItem.Project = Projects.FirstOrDefault();
            initItem.Files.Add(newFile);
            initItem.Tags = "mojo;josie;kibby";
            if (user != null) initItem.Assignee = user;
            else System.Console.WriteLine("NULL USER");
            TodoItems.Add(initItem);
            SaveChanges();
        }


        /// <summary>
        /// [Dev] Seed the Project table with an item.
        /// </summary>
        public void DevSeedProject() {
            var newFile = new FileRef("anotherTest/file.png");

            var initProject = new Project();
            initProject.Name = "Test Project";
            initProject.Tags = "test;demo;ignore;";
            initProject.Description = "A Test Project";
            initProject.Files.Add(newFile);
            initProject.ValidStatuses = StatusSets.FirstOrDefault();
            Projects.Add(initProject);
            SaveChanges();
        }


        /// <summary>
        /// [Dev] Seed the users table with an item.
        /// </summary>
        public void DevSeedUser() {
            var newUser = new User();
            newUser.Name = "Dev Test User";
            Users.Add(newUser);
            SaveChanges();
        }


    }


}