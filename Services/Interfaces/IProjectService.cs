using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services.Interfaces {

    public interface IProjectService {
        
        IActionResult GetAllProjects();

        IActionResult GetProject(long projectId);

        IActionResult GetProjectByUser(long userId, bool includeUnowned);
    }
}