
using Microsoft.AspNetCore.Mvc;

using Crunchy.Services.Interfaces;

namespace Crunchy.Controllers {

    [Route("api/users")]
    public class UserController : CrunchyController {

        public IUserService UserService { get; set; }

        public UserController(IUserService userService) {
            UserService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
            => UserService.GetAllUsers();


        [HttpGet("{id}")]
        public IActionResult GetUser(long id)
            => UserService.GetUser(id);
    }
}