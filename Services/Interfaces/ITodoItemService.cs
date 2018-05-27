using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;


namespace Crunchy.Services.Interfaces {
    
    public interface ITodoItemService {

        IActionResult GetAllTodoItems();

        IActionResult GetTodoItem(long id);

        IActionResult GetTodoItemByUser(long userId, bool includeUnowned);

        //Task<IActionResult> GetAllTodoItemsAsync();
    }
}