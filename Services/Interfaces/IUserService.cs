using Microsoft.AspNetCore.Mvc;

namespace Crunchy.Services.Interfaces {

    public interface IUserService {

        IActionResult GetAllUsers();

        IActionResult GetUser(long userId);
    }
}