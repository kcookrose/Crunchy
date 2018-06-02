using System.IO;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Crunchy.Models;
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


        [HttpGet("{id}", Name="GetUser")]
        public IActionResult GetUser(long id)
            => UserService.GetUser(id);


        [HttpPost]
        public IActionResult CreateUser() {
            using (StreamReader bodyReader = new StreamReader(Request.Body)) {
                string json = bodyReader.ReadToEnd();
                return UserService.CreateUser(json);
            }
        }


        [HttpPut("{userId}")]
        public IActionResult UpdateUser(long userId) {
            using (StreamReader bodyReader = new StreamReader(Request.Body)) {
                string json = bodyReader.ReadToEnd();
                return UserService.UpdateUser(userId, json);
            }            
        }


        [HttpDelete("{userId}")]
        public IActionResult DeleteUser(long userId)
            => UserService.DeleteUser(userId);
    }
}