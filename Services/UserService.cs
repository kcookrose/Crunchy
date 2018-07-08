using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

namespace Crunchy.Services {

    public class UserService : CrunchyService, IUserService { // TODO: Update to use join table

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
                if (user != null) {
                    var entry = context.Entry(user);
                    context.EnsureDeepLoaded(entry);
                    return Ok(GetDetailedModel(user));
                }
                return NotFound();
            }
        }


        public IActionResult CreateUser(string newUserJson) {
            User newUser = UserFromJson(newUserJson);
            if (newUser == null) return BadRequest();
            using (var context = new TodoContext()) {
                if (newUser.DefaultProject != null)
                    newUser.DefaultProject = context.Projects.Find(newUser.DefaultProjectId);
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


        public IActionResult DeleteUser(long userId) {
            using (var context = new TodoContext()) {
                User victim = context.Users.Find(userId);
                if (victim == null) return BadRequest();
                context.Users.Remove(victim);
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