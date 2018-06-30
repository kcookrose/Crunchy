using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Crunchy.Services.Interfaces;

namespace Crunchy.Controllers {

    [Route("api/filerefs")]
    public class FileRefController : CrunchyController {

        public IFileRefService FileRefService { get; set; }

        public FileRefController(IFileRefService fileRefService) {
            FileRefService = fileRefService;
        }


        [HttpGet]
        public IActionResult GetAllFileRefs()
            => FileRefService.GetAllFileRefs();

        
        [HttpGet("{fId}", Name="GetFileRef")]
        public IActionResult GetFileRef(long fId)
            => FileRefService.GetFileRef(fId);
        

        [HttpPost]
        public IActionResult CreateFileRef() {
            using (var reader = new StreamReader(Request.Body)) {
                string body = reader.ReadToEnd();
                return FileRefService.CreateFileRef(body);
            }
        }


        [HttpPut("{fId}")]
        public IActionResult UpdateFileRef(long fId) {
            using (var reader = new StreamReader(Request.Body)) {
                string body = reader.ReadToEnd();
                return FileRefService.UpdateFileRef(fId, body);
            }
        }


        [HttpDelete("{fId}")]
        public IActionResult DeleteFileRef(long fId)
            => FileRefService.DeleteFileRef(fId);

    }
}