using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Crunchy.Models;


namespace Crunchy.Controllers {

    [Route("api/[controller]")]
    public class TodoController : ControllerBase {
        
        private readonly TodoContext _context;

        public TodoController(TodoContext context) {
            _context = context;

            if (_context.TodoItems.Count() == 0) {
                var initItem = new TodoItem();
                initItem.Name = "Init Item";
                initItem.Files = new List<FileRef>();
                initItem.Files.Add(new FileRef { RepoUrl = "test/file.jpg" });
                _context.TodoItems.Add(initItem);
                _context.SaveChanges();
            }

            if (_context.ProjectItems.Count() == 0) {
                var initProject = new ProjectItem();
                initProject.Name = "Test Project";
                initProject.Tags = "test;demo;ignore;";
                initProject.Description = "A Test Project";
                initProject.Files.Add(new FileRef { RepoUrl = "anotherTest/file.png"});
                _context.ProjectItems.Add(initProject);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<TodoItem> GetAll() {
            return _context.TodoItems.ToList();
        }


        [HttpGet("projects", Name = "GetProjects")]
        public List<ProjectItem> GetAllProjects() {
            return _context.ProjectItems.ToList();
        }


        [HttpGet("{Tid}", Name = "GetTodo")]
        public IActionResult GetById(long Tid) {
            var item = _context.TodoItems.Find(Tid);
            if (item == null) {
                return NotFound();
            }
            return Ok(item);
        }


    }
}
