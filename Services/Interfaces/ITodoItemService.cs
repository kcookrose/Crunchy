using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;


namespace Crunchy.Services.Interfaces {
    
    public interface ITodoItemService {

        IActionResult GetAllTodoItems();

        IActionResult GetTodoItem(long id);

        IActionResult CreateTodoItem(string json);

        //IActionResult GetTodoItemByUser(long userId, bool includeUnowned);

    }
}