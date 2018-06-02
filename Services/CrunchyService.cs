using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services {

    public class CrunchyService {
        // Because I'm lazy
        protected static OkObjectResult Ok(object obj) => new OkObjectResult(obj);
        protected static OkResult Ok() => new OkResult();
        protected static NotFoundResult NotFound() => new NotFoundResult();
        protected static CreatedAtRouteResult CreatedAtRoute(string route, object formInfo, object newObj)
            => new CreatedAtRouteResult(route, formInfo, newObj);
        protected static NoContentResult NoContent() => new NoContentResult();
        protected static BadRequestResult BadRequest() => new BadRequestResult();
    }
}