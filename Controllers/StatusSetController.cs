using System.IO;

using Microsoft.AspNetCore.Mvc;

using Crunchy.Services.Interfaces;

namespace Crunchy.Controllers {

    [Route("api/statussets")]
    public class StatusSetController : CrunchyController {

        public IStatusSetService StatusSetService { get; set; }

        public StatusSetController(IStatusSetService statusSetService) {
            StatusSetService = statusSetService;
        }


        [HttpGet]
        public IActionResult GetAllStatusSets()
            => StatusSetService.GetAllStatusSets();

        
        [HttpGet("{ssId}", Name="GetStatusSet")]
        public IActionResult GetStatusSet(long ssId)
            => StatusSetService.GetStatusSet(ssId);
        

        [HttpPost]
        public IActionResult CreateStatusSet() {
            using (StreamReader bodyReader = new StreamReader(Request.Body)) {
                string json = bodyReader.ReadToEnd();
                return StatusSetService.CreateStatusSet(json);
            }
        }


        [HttpPut("{ssId}")]
        public IActionResult UpdateStatusSet(long ssId) {
            using (StreamReader bodyReader = new StreamReader(Request.Body)) {
                string json = bodyReader.ReadToEnd();
                return StatusSetService.UpdateStatusSet(ssId, json);
            }
        }


        [HttpDelete("{ssId}")]
        public IActionResult DeleteStatusSet(long ssId)
            => StatusSetService.DeleteStatusSet(ssId);
    }
}