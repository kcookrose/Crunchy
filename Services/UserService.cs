using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class UserService : CrunchyService, IUserService {

        public IActionResult GetAllUsers() {
            using (var context = new TodoContext()) {
                var res = context.Users
                        .Select(user => GetShortModel(user))
                        .ToArray();
                return Ok(res);
            }
        }


        public IActionResult GetUser(long userId) {
            using (var context = new TodoContext()) {
                var user = context.Users.Find(userId);
                if (user != null)
                    return Ok(GetDetailedModel(user));
                return NotFound();
            }
        }


        public IActionResult CreateUser(string newUserJson) {
            var newUserObj = new {
                Name = "",
                DefaultProjectId = 0L
            };
            newUserObj = JsonConvert.DeserializeAnonymousType(newUserJson, newUserObj);
            using (var context = new TodoContext()) {
                context.DevSeedDatabase();
                User newUser = new User();
                newUser.Name = newUserObj.Name;
                foreach (var proj in context.Projects.ToArray())
                    System.Console.WriteLine("pid: " + proj.Pid);
                Project project = context.Projects.Find(newUserObj.DefaultProjectId);
                if (project != null)
                    newUser.DefaultProject = project;
                context.Users.Add(newUser);
                context.SaveChanges();
                return CreatedAtRoute("GetUser", new {id = newUser.Uid}, GetDetailedModel(newUser));
            }
        }


        public object GetShortModel(User user) {
            return new {
                Uid = user.Uid,
                Name = user.Name
            };
        }


        public object GetDetailedModel(User user) {
            return new {
                Uid = user.Uid,
                Name = user.Name,
                DefaultProjectId = user.DefaultProjectId
            };
        }
    }
}