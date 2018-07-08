using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class ProjectService : CrunchyService, IProjectService {

        public IActionResult GetAllProjects() {
            using (var context = new TodoContext()) {
                var res = new List<object>();
                foreach (var proj in context.Projects)
                    res.Add(GetShortModel(context, proj));
                return Ok(res);
            }
        }


        public IActionResult GetProject(long projectId) {
            using (var context = new TodoContext()) {
                var project = context.Projects.Find(projectId);
                if (project != null) {
                    return Ok(GetDetailedModel(context, project));
                }    
                return NotFound();
            }
        }


        public IActionResult CreateProject(string projectJson) {
            Tuple<Project, IList<User>> newProjTup = ProjectFromJson(projectJson);
            if (newProjTup == null) return BadRequest();
            Project newProj = newProjTup.Item1;
            using (var context = new TodoContext()) {
                foreach (var file in newProj.Files)
                    context.FileRefs.Add(file);
                foreach (var newOwner in newProjTup.Item2)
                    ProjectOwner.Join(context, newProj, newOwner);
                context.ChangeTracker.TrackGraph(newProj, (node => node.Entry.State = node.Entry.IsKeySet ? EntityState.Unchanged : EntityState.Added));
                context.SaveChanges();
                return CreatedAtRoute("GetProject", new { id = newProj.Pid}, GetDetailedModel(context, newProj)); // FIXME: Doesn't give correct response data
            }
        }


        public IActionResult UpdateProject(long pId, string projectJson) {
            Tuple<Project, IList<User>> newProjTup = ProjectFromJson(projectJson);
            if (newProjTup == null) return BadRequest();
            Project newProj = newProjTup.Item1;
            using (var context = new TodoContext()) {
                Project oldProj = context.Projects.Find(pId);
                if (oldProj == null) return NotFound();
                var entry = context.Entry(oldProj);
                context.EnsureDeepLoaded(entry);
                foreach (var file in oldProj.Files)
                    context.FileRefs.Remove(file);
                oldProj.Files.Clear();

                foreach (var projOwner in oldProj.OwnerUsers)
                    projOwner.Unjoin(context);

                oldProj.Name = newProj.Name;
                oldProj.Description = newProj.Description;
                if (newProj.ValidStatuses != null)
                    oldProj.ValidStatuses = context.StatusSets.Find(newProj.ValidStatuses.Id);
                else
                    oldProj.ValidStatuses = null;
                foreach (var newOwner in newProjTup.Item2)
                    ProjectOwner.Join(context, oldProj, newOwner);
                oldProj.Tags = newProj.Tags;
                foreach (var file in newProj.Files) {
                    context.FileRefs.Add(file);
                    newProj.Files.Add(file);
                }
                context.SaveChanges();
                return NoContent();
            }
        }


        public IActionResult DeleteProject(long pId) {
            using (var context = new TodoContext()) {
                Project oldProj = context.Projects.Find(pId);
                if (oldProj == null) return NotFound();
                context.EnsureDeepLoaded(context.Entry(oldProj));
                foreach (var file in oldProj.Files) // FIXME: Doesn't trigger physical deletion
                    context.FileRefs.Remove(file);
                for (int i = oldProj.OwnerUsers.Count-1; i >= 0; i--)
                    oldProj.OwnerUsers[i].Unjoin(context);
                oldProj.Files.Clear();
                context.Projects.Remove(oldProj);
                context.SaveChanges();
                return NoContent();
            }
        }


        /// <summary>
        /// Generate an abbreviated version of the project
        /// </summary>
        /// <param name="project">The project to parse</param>
        /// <returns>The abbreviated project, as an anonymous type</returns>
        public object GetShortModel(TodoContext context, Project project) {
            var entry = context.Entry(project);
            context.EnsureDeepLoaded(entry);
            long[] userIds = project.OwnerUsers.Select(owner => owner.UserId).ToArray();
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
        public object GetDetailedModel(TodoContext context, Project project) {
            var entry = context.Entry(project);
            context.EnsureDeepLoaded(entry);
            long validStatuses = -1;
            if (project.ValidStatuses != null)
                validStatuses = project.ValidStatuses.Id;
            long[] userIds = project.OwnerUsers.Select(owner => owner.UserId).ToArray();
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


        public Tuple<Project, IList<User>> ProjectFromJson(string projectJson) {
            var newProj = new {
                Pid = -1L,
                Name = "",
                Description = "",
                StatusSetId = -1L,
                OwnerUserIds = new List<long>(),
                Tags = "",
                Files = new List<string>()
            };
            newProj = JsonConvert.DeserializeAnonymousType(projectJson, newProj);
            if (String.IsNullOrEmpty(newProj.Name)) {
                System.Console.WriteLine("Bad name");
                return null;
            }
            using (var context = new TodoContext()) {
                if (newProj.StatusSetId > 0) {
                    if (context.StatusSets.Find(newProj.StatusSetId) == null) {
                        System.Console.WriteLine("Invalid status set");
                        return null;
                    }    
                }
                if (newProj.Pid > 0) {
                    if (context.Projects.Find(newProj.Pid) == null) {
                        System.Console.WriteLine("Invalid ID: " + newProj.Pid);
                        return null;
                    }
                }
                foreach (long uId in newProj.OwnerUserIds) {
                    if (context.Users.Find(uId) == null) {
                        System.Console.WriteLine("Invalid user: " + uId);
                        return null;
                    }
                }
                Project res = new Project();
                res.Pid = newProj.Pid;
                res.Name = newProj.Name;
                res.Description = newProj.Description;
                res.ValidStatuses = context.StatusSets.Find(newProj.StatusSetId);
                //res.OwnerUsers = new List<ProjectOwner>();
                IList<User> owners = newProj.OwnerUserIds
                    .Select(newUser => context.Users.Find(newUser))
                    .ToArray();
                res.Tags = newProj.Tags;
                res.Files = new List<FileRef>();
                foreach (var file in newProj.Files) {
                    res.Files.Add(new FileRef(file));
                }
                return Tuple.Create<Project, IList<User>>(res, owners);
            }
        }
    }
}