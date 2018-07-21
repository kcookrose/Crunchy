using System.IO;

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
        [HttpGet("{id}", Name="GetTodoItem")]
        public IActionResult GetTodoItem(long id)
            => TodoItemService.GetTodoItem(id);

        
        /// <summary>
        /// Create a new Todo Item.
        /// </summary>
        /// <returns>The created Todo Item, if successful</returns>
        [HttpPost]
        public IActionResult CreateTodoItem() {
            using (var stream = new StreamReader(Request.Body)) {
                string json = stream.ReadToEnd();
                return TodoItemService.CreateTodoItem(json);
            }
        }


        /// <summary>
        /// Update an existing Todo Item.
        /// </summary>
        /// <param name="id">The id of the Todo Item to update</param>
        /// <returns>The updated Todo Item, if succesful</returns>
        [HttpPut("{id}")]
        public IActionResult UpdateTodoItem(long id) {
            using (var reader = new StreamReader(Request.Body)) {
                string json = reader.ReadToEnd();
                return TodoItemService.UpdateTodoItem(id, json);
            }
        } 


        /// <summary>
        /// Get a set of todo items visible by the specified user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A set of visible todo items</returns>
        /*[HttpGet("byuser/{userId}")]
        public IActionResult GetTodoItemByUser(long userId,
                [FromQuery(Name="includeunowned")]bool includeUnowned)
            => TodoItemService.GetTodoItemByUser(userId, includeUnowned);*/
    }
}