using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services.Interfaces {

    public interface IProjectService {
        
        IActionResult GetAllProjects();

        IActionResult GetProject(long projectId);

        IActionResult CreateProject(string projectJson);

        IActionResult UpdateProject(long pId, string projectJson);

        IActionResult DeleteProject(long pId);
    }
}