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
            using (var context = new TodoContext()) {
                return context.Projects.Select(project => GetShortModel(project)).ToArray();
            }
        }


        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id">The id of the desired project</param>
        /// <returns>The project, or null if not found</returns>
        [HttpGet("projects/{id}")]
        public IActionResult GetProject(long id) {
            using (var context = new TodoContext()) {
                var project = context.Projects.Find(id);
                if (project != null)
                    return Ok(GetDetailedModel(project));
                return NotFound();
            }
        }


        /// <summary>
        /// Get project by user
        /// </summary>
        /// <param name="userId">The ID of the user to query</param>
        /// <returns>The set of projects with the specified user as an owner</returns>
        [HttpGet("projects/byUser/{userId}")]
        public IActionResult GetProjectByUser(long userId) {
            using (var context = new TodoContext()) {
                var filteredProjects = context.Projects
                    //.Include(project => project.OwnerUsers)
                    .Where(project => project.OwnerUsers
                        .Any(user => user.Uid == userId));
                var formattedProjects = filteredProjects
                    .Select(project => GetShortModel(project))
                    .ToArray();
                if (formattedProjects.Length > 0)
                    return Ok(formattedProjects);
            }
            return NotFound();
        }


        /// <summary>
        /// Generate an abbreviated version of the project
        /// </summary>
        /// <param name="project">The project to parse</param>
        /// <returns>The abbreviated project, as an anonymous type</returns>
        public object GetShortModel(Project project) {
            using (var context = new TodoContext()) {
                var entry = context.Entry(project);
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
        }
        

        /// <summary>
        /// Generate a more detailed version of the project
        /// </summary>
        /// <param name="project">The project to parse</param>
        /// <returns>The detailed object, as an anonymous type</returns>
        public object GetDetailedModel(Project project) {
            using (var context = new TodoContext()) {
                var entry = context.Entry(project);
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
}