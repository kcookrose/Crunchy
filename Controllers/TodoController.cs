using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Linq;
using Crunchy.Models;

namespace Crunchy.Controllers {

    [Route("api")]
    public partial class TodoController : ControllerBase {
        

        public TodoController(TodoContext context) {

            if (context.StatusSets.Count() == 0) DevSeedStatuses();
            if (context.Users.Count() == 0) DevSeedUser();
            if (context.Projects.Count() == 0) DevSeedProject();
            if (context.TodoItems.Count() == 0) DevSeedTodoItem();

        }

        /// <summary>
        /// [Dev] Seed the Status table with statuses
        /// </summary>
        public void DevSeedStatuses() {
            using (var context = new TodoContext()) {
                var newStatusSet = new StatusSet("Test Status Set");
                //foreach (var newStatus in new string []{"TODO", "In Progress", "Complete"})
                 //   newStatusSet.Add(newStatus);
                newStatusSet.Add("TODO");
                newStatusSet.Add("In Progress");
                newStatusSet.Add("Complete");
                
                context.StatusSets.Add(newStatusSet);
                context.SaveChanges();
            }
            using (var context = new TodoContext()) {
                System.Console.WriteLine("Validating Statues...");
                System.Console.WriteLine("--Count: " + context.StatusSets.Count());
                foreach (var ss in context.StatusSets) {
                    EnsureDeepLoaded(context.Entry(ss));
                    System.Console.WriteLine("---" + ss.ToString());
                }
            }
        }


        /// <summary>
        /// [Dev] Seed the Todo table with an item.
        /// </summary>
        public void DevSeedTodoItem() {
            using (var context = new TodoContext()) {
                var newFile = new FileRef("test/file.jpg");

                var user = context.Users.FirstOrDefault();
                var statusSet = context.StatusSets.FirstOrDefault();

                var project = context.Projects.FirstOrDefault();
                if (project == null) {
                    System.Console.WriteLine(" !!! Tried to create Todo without project");
                    return;
                }
                EnsureDeepLoaded(context.Entry(project));
                EnsureDeepLoaded(context.Entry(project.ValidStatuses));
                var initItem = new TodoItem(project, project.ValidStatuses.Statuses[0]);
                initItem.Name = "Init Item";
                initItem.Project = context.Projects.FirstOrDefault();
                initItem.Files.Add(newFile);
                initItem.Tags = "mojo;josie;kibby";
                if (user != null) initItem.Assignee = user;
                else System.Console.WriteLine("NULL USER");
                context.TodoItems.Add(initItem);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// [Dev] Seed the Project table with an item.
        /// </summary>
        public void DevSeedProject() {
            using (var context = new TodoContext()) {
                var newFile = new FileRef("anotherTest/file.png");

                var initProject = new Project();
                initProject.Name = "Test Project";
                initProject.Tags = "test;demo;ignore;";
                initProject.Description = "A Test Project";
                initProject.Files.Add(newFile);
                initProject.ValidStatuses = context.StatusSets.FirstOrDefault();
                context.Projects.Add(initProject);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// [Dev] Seed the users table with an item.
        /// </summary>
        public void DevSeedUser() {
            using (var context = new TodoContext()) {
                var newUser = new User();
                newUser.Name = "Dev Test User";
                context.Users.Add(newUser);
                context.SaveChanges();
            }
        }


        /// <summary>
        /// Deep load all entries on the item
        /// </summary>
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

