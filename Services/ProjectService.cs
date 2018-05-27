using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class ProjectService : CrunchyService, IProjectService {

        public IActionResult GetAllProjects() {
            using (var context = new TodoContext()) {
                var res = context.Projects
                    .Select(project => GetShortModel(project))
                    .ToArray();
                return Ok(res);
            }
        }


        public IActionResult GetProject(long projectId) {
            using (var context = new TodoContext()) {
                var project = context.Projects.Find(projectId);
                if (project != null)
                    return Ok(GetDetailedModel(project));
                return NotFound();
            }
        }


        public IActionResult GetProjectByUser(long userId, bool includeUnowned) {
            using (var context = new TodoContext()) {
                var filteredProjects = context.Projects
                    .Where(project => project.OwnerUsers.Count == 0 ||
                        project.OwnerUsers.Any(user => user.Uid == userId));
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