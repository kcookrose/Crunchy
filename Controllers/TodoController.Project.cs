using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Crunchy.Models;


/*
 * This file should contain controller elements dedicated to working with
 * Projects
 */

namespace Crunchy.Controllers {

    public partial class TodoController : ControllerBase {

        /// <summary>
        /// Get all projects.
        /// </summary>
        [HttpGet("projects", Name = "GetProjects")]
        public List<Project> GetAllProjects() {
            return _context.Projects.ToList();
        }


        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id">The id of the desired project</param>
        /// <returns>The project, or null if not found</returns>
        [HttpGet("projects/{id}")]
        public IActionResult GetProject(long id) {
            var project = _context.Projects.Find(id);
            if (project != null) {
                foreach (var coll in _context.Entry(project).Collections)
                    coll.Load();
                return Ok(project);
            }
            return NotFound();
        }


    }
}