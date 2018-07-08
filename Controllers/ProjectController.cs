using System.IO;
using Microsoft.AspNetCore.Mvc;

using Crunchy.Services.Interfaces;


namespace Crunchy.Controllers {

    [Route("api/projects")]
    public class ProjectController : CrunchyController {

        public IProjectService ProjectService { get; set; }

        public ProjectController(IProjectService projectService) {
            ProjectService = projectService;
        }


        /// <summary>
        /// Get all projects.
        /// </summary>
        [HttpGet(Name = "GetProjects")]
        public IActionResult GetAllProjects()
            => ProjectService.GetAllProjects();


        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id">The id of the desired project</param>
        /// <returns>The project, or null if not found</returns>
        [HttpGet("{id}", Name = "GetProject")]
        public IActionResult GetProject(long id)
            => ProjectService.GetProject(id);


        /// <summary>
        /// Get project by user
        /// </summary>
        /// <param name="userId">The ID of the user to query</param>
        /// <returns>The set of projects with the specified user as an owner</returns>
        /// TODO: Re-add interface
        //[HttpGet("byUser/{userId}")]
        //public IActionResult GetProjectByUser(long userId,
        //        [FromQuery(Name="includeunowned")]bool includeUnowned)
        //    => ProjectService.GetProjectByUser(userId, includeUnowned);

        
        [HttpPost]
        public IActionResult CreateProject() {
            using (var reader = new StreamReader(Request.Body)) {
                string json = reader.ReadToEnd();
                return ProjectService.CreateProject(json);
            }
        }


        [HttpPut("{pId}")]
        public IActionResult UpdateProject(long pId) {
            using (var reader = new StreamReader(Request.Body)) {
                string json = reader.ReadToEnd();
                return ProjectService.UpdateProject(pId, json);
            }
        }


        [HttpDelete("{pId}")]
        public IActionResult DeleteProject(long pId)
            => ProjectService.DeleteProject(pId);
    }
}