using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

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
        public IActionResult GetAllProjects([FromServices]IProjectService projService)
            => projService.GetAllProjects();


        /// <summary>
        /// Get project by id.
        /// </summary>
        /// <param name="id">The id of the desired project</param>
        /// <returns>The project, or null if not found</returns>
        [HttpGet("projects/{id}")]
        public IActionResult GetProject(long id,
                [FromServices]IProjectService projService)
            => projService.GetProject(id);


        /// <summary>
        /// Get project by user
        /// </summary>
        /// <param name="userId">The ID of the user to query</param>
        /// <returns>The set of projects with the specified user as an owner</returns>
        [HttpGet("projects/byUser/{userId}")]
        public IActionResult GetProjectByUser(long userId,
                [FromQuery(Name="includeunowned")]bool includeUnowned,
                [FromServices]IProjectService projService)
            => projService.GetProjectByUser(userId, includeUnowned);

    }
}