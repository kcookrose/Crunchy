using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services.Interfaces {

    public interface IFileRefService {

        IActionResult GetAllFileRefs();

        IActionResult GetFileRef(long fId);

        IActionResult CreateFileRef(string fileRefJson);

        IActionResult UpdateFileRef(long fId, string fileRefJson);

        IActionResult DeleteFileRef(long fId);

    }
}