using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                foreach (var newStatus in new string []{"TODO", "In Progress", "Complete"})
                    newStatusSet.Add(newStatus);
                context.StatusSets.Add(newStatusSet);
                context.SaveChanges();
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
                var initItem = new TodoItem(project, project.ValidStatuses.Statuses[0]);
                initItem.Name = "Init Item";
                initItem.Project = context.Projects.FirstOrDefault();
                initItem.Files.Add(newFile);
                if (user != null) initItem.Assignee = user;
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
    }
}

