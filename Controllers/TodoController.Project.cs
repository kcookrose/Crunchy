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
        public object[] GetAllProjects() {
            return _context.Projects.Select(project => GetShortModel(project)).ToArray();
        }


        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id">The id of the desired project</param>
        /// <returns>The project, or null if not found</returns>
        [HttpGet("projects/{id}")]
        public IActionResult GetProject(long id) {
            var project = _context.Projects.Find(id);
            if (project != null)
                return Ok(GetDetailedModel(project));
            return NotFound();
        }


        /// <summary>
        /// Generate an abbreviated version of the project
        /// </summary>
        /// <param name="project">The project to parse</param>
        /// <returns>The abbreviated project, as an anonymous type</returns>
        public object GetShortModel(Project project) {
            var entry = _context.Entry(project);
            if (entry.State != EntityState.Detached) {
                entry.Collection("OwnerUsers").Load();
            }
            long[] userIds = project.OwnerUsers.Select(owner => owner.Uid).ToArray();
            return new {
                Pid = project.Pid,
                Name = project.Name,
                OwnerUserIds = userIds,
                Tags = project.Tags
            };
        }
        

        /// <summary>
        /// Generate a more detailed version of the project
        /// </summary>
        /// <param name="project">The project to parse</param>
        /// <returns>The detailed object, as an anonymous type</returns>
        public object GetDetailedModel(Project project) {
            var entry = _context.Entry(project);
            if (entry.State != EntityState.Detached) {
                entry.Navigation("ValidStatuses").Load();
                entry.Collection("OwnerUsers").Load();
                entry.Collection("Files").Load();
            }
            long validStatuses = -1;
            if (project.ValidStatuses != null)
                validStatuses = project.ValidStatuses.Id;
            long[] userIds = project.OwnerUsers.Select(owner => owner.Uid).ToArray();
            string[] files = project.Files.Select(file => file.RepoUrl).ToArray();
            return new {
                Pid = project.Pid,
                Name = project.Name,
                Description = project.Description,
                StatusSetId = validStatuses,
                OwnerUserIds = userIds,
                Tags = project.Tags,
                Files = files
            };
        }
    }
}