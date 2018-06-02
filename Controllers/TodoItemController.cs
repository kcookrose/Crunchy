using Microsoft.AspNetCore.Mvc;

using Crunchy.Services.Interfaces;

namespace Crunchy.Controllers {

    [Route("api/todoitems")]
    public class TodoItemController : CrunchyController {

        public ITodoItemService TodoItemService { get; set; }

        public TodoItemController(ITodoItemService itemService) {
            TodoItemService = itemService;
        }


        /// <summary>
        /// Get all todo items
        /// NOTE: Should probably be removed in relase
        /// </summary>
        [HttpGet]
        public IActionResult GetAllTodoItems()
            => TodoItemService.GetAllTodoItems();


        /// <summary>
        /// Get a todoItem by its ID.
        /// </summary>
        /// <param name="id">The ID of the desired todoitem</param>
        /// <returns>The requested todo item</returns>
        [HttpGet("{id}")]
        public IActionResult GetTodoItem(long id)
            => TodoItemService.GetTodoItem(id);


        /// <summary>
        /// Get a set of todo items visible by the specified user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A set of visible todo items</returns>
        [HttpGet("byuser/{userId}")]
        public IActionResult GetTodoItemByUser(long userId,
                [FromQuery(Name="includeunowned")]bool includeUnowned)
            => TodoItemService.GetTodoItemByUser(userId, includeUnowned);
    }
}