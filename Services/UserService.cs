using System;
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
            User newUser = UserFromJson(newUserJson);
            if (newUser == null) return BadRequest();
            using (var context = new TodoContext()) {
                context.Users.Add(newUser);
                context.SaveChanges();
                return CreatedAtRoute("GetUser", new {id = newUser.Uid},
                        GetDetailedModel(newUser));
            }
        }


        public IActionResult UpdateUser(long userId, string userJson) {
            User user = UserFromJson(userJson);
            if (user == null) return BadRequest();
            using (var context = new TodoContext()) {
                User origUser = context.Users.Find(userId);
                if (origUser == null) return NotFound();
                origUser.Name = user.Name;
                if (user.DefaultProject != null)
                    origUser.DefaultProject = context.Projects
                            .Find(user.DefaultProject.Pid);
                context.Update(origUser);
                context.SaveChanges();
                return NoContent();
            }
        }


        public User UserFromJson(string json) {
            var newUserObj = new {
                Name = "",
                DefaultProjectId = 0L
            };
            try {
                newUserObj = JsonConvert.DeserializeAnonymousType(json, newUserObj);
            } catch (JsonReaderException) {
                System.Console.WriteLine("Failed to parse json: " + json);
                return null;
            }
            if (String.IsNullOrEmpty(newUserObj.Name) ||
                    newUserObj.DefaultProjectId == 0)
                return null;
            using (var context = new TodoContext()) {
                context.DevSeedDatabase();
                User newUser = new User();
                newUser.Name = newUserObj.Name;
                Project project = context.Projects.Find(newUserObj.DefaultProjectId);
                if (project != null)
                    newUser.DefaultProject = project;
                return newUser;
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