using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Crunchy.Models;

namespace Crunchy.Controllers {

    [Route("[controller]")]
    public partial class DevController : ControllerBase {
        
        public DevController() { }

        [HttpGet("resetdb")]
        public IActionResult ResetDatabase(TodoContext context) {
            context.DevSeedDatabase();
            return Ok();
        }


        [HttpGet]
        public IActionResult IsDevMode() {
            return Ok(true);
        }
    }
}

