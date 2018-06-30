using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class FileRefService : CrunchyService, IFileRefService {


        public IActionResult GetAllFileRefs() {
            using (var context = new TodoContext()) {
                List<object> res = new List<object>();
                foreach (var fileRef in context.FileRefs)
                    res.Add(fileRef);
                return Ok(res);
            }
        }


        public IActionResult GetFileRef(long fId) {
            using (var context = new TodoContext()) {
                FileRef res = context.FileRefs.Find(fId);
                if (res != null) return Ok(res);
                return NotFound();
            }
        }


        public IActionResult CreateFileRef(string fileRefJson) {
            using (var context = new TodoContext()) {
                FileRef res = JsonConvert.DeserializeObject<FileRef>(fileRefJson);
                if (String.IsNullOrEmpty(res.RepoUrl)) return BadRequest();
                context.FileRefs.Add(res);
                context.SaveChanges();
                return CreatedAtRoute("GetFileRef", new {Id = res.Id}, res);
            }
        }


        public IActionResult DeleteFileRef(long fId) {
            using (var context = new TodoContext()) {
                FileRef res = context.FileRefs.Find(fId);
                if (res == null) return NotFound();
                context.FileRefs.Remove(res);
                context.SaveChanges();
                return NoContent();
            }
        }


        public IActionResult UpdateFileRef(long fId, string fileRefJson) {
            using (var context = new TodoContext()) {
                FileRef oldRef = context.FileRefs.Find(fId);
                if (oldRef == null) return NotFound();
                FileRef newRef = JsonConvert.DeserializeObject<FileRef>(fileRefJson);
                if (newRef == null || String.IsNullOrEmpty(newRef.RepoUrl) ||
                    newRef.Id != fId) return BadRequest();
                oldRef.RepoUrl = newRef.RepoUrl;
                context.SaveChanges();
                return NoContent();
            }
        }
    }
}