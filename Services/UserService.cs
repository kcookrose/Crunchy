using System.Linq;

using Microsoft.AspNetCore.Mvc;

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
                DefaultProject = user.DefaultProject
            };
        }
    }
}