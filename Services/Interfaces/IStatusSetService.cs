using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services.Interfaces {

    public interface IStatusSetService {
        
        IActionResult GetAllStatusSets();

        IActionResult GetStatusSet(long ssId);

        IActionResult CreateStatusSet(string newStatusSetJson);

        IActionResult UpdateStatusSet(long ssId, string statusSetJson);

        IActionResult DeleteStatusSet(long ssId);
    }
}