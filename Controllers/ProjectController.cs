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
        [HttpGet("{id}")]
        public IActionResult GetProject(long id)
            => ProjectService.GetProject(id);


        /// <summary>
        /// Get project by user
        /// </summary>
        /// <param name="userId">The ID of the user to query</param>
        /// <returns>The set of projects with the specified user as an owner</returns>
        [HttpGet("byUser/{userId}")]
        public IActionResult GetProjectByUser(long userId,
                [FromQuery(Name="includeunowned")]bool includeUnowned)
            => ProjectService.GetProjectByUser(userId, includeUnowned);
    }
}