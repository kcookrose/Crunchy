using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services {

    public class CrunchyService {
        // Because I'm lazy
        protected static OkObjectResult Ok(object obj) => new OkObjectResult(obj);
        protected static OkResult Ok() => new OkResult();
        protected static NotFoundResult NotFound() => new NotFoundResult();
    }
}