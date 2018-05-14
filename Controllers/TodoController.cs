using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Crunchy.Models;


namespace Crunchy.Controllers {

    [Route("api")]
    public partial class TodoController : ControllerBase {
        

        private readonly TodoContext _context;

        public TodoController(TodoContext context) {
            _context = context;

            if (_context.StatusSets.Count() == 0) DevSeedStatuses();
            if (_context.Users.Count() == 0) DevSeedUser();
            if (_context.Projects.Count() == 0) DevSeedProject();
            if (_context.TodoItems.Count() == 0) DevSeedTodoItem();

        }

        /// <summary>
        /// [Dev] Seed the Status table with statuses
        /// </summary>
        public void DevSeedStatuses() {
            var newStatusSet = new StatusSet("Test Status Set");
            foreach (var newStatus in new string []{"TODO", "In Progress", "Complete"})
                newStatusSet.Add(newStatus);
            _context.StatusSets.Add(newStatusSet);
            _context.SaveChanges();
        }


        /// <summary>
        /// [Dev] Seed the Todo table with an item.
        /// </summary>
        public void DevSeedTodoItem() {
            var newFile = new FileRef("test/file.jpg");

            var user = _context.Users.FirstOrDefault();
            var statusSet = _context.StatusSets.FirstOrDefault();

            var project = _context.Projects.FirstOrDefault();
            if (project == null) {
                System.Console.WriteLine(" !!! Tried to create Todo without project");
                return;
            }
            var initItem = new TodoItem(project, project.ValidStatuses.Statuses[0]);
            initItem.Name = "Init Item";
            initItem.Project = _context.Projects.FirstOrDefault();
            initItem.Files.Add(newFile);
            if (user != null) initItem.Assignee = user;
            _context.TodoItems.Add(initItem);
            _context.SaveChanges();
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
            initProject.ValidStatuses = _context.StatusSets.FirstOrDefault();
            _context.Projects.Add(initProject);
            _context.SaveChanges();
        }


        /// <summary>
        /// [Dev] Seed the users table with an item.
        /// </summary>
        public void DevSeedUser() {
            var newUser = new User();
            newUser.Name = "Dev Test User";
            _context.Users.Add(newUser);
            _context.SaveChanges();
        }
    }
}

