using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Crunchy.Models;
using Crunchy.Services.Interfaces;

/*
 * This file should contain controller elements dedicated to working with
 * Projects
 */

namespace Crunchy.Controllers {

    public partial class TodoController : ControllerBase {


        /// <summary>
        /// Get all todo items
        /// NOTE: Should probably be removed in relase
        /// </summary>
        [HttpGet("todoItems")]
        public IActionResult GetAllTodoItems(
                [FromServices]ITodoItemService itemService)
            => itemService.GetAllTodoItems();


        /// <summary>
        /// Get a todoItem by its ID.
        /// </summary>
        /// <param name="id">The ID of the desired todoitem</param>
        /// <returns>The requested todo item</returns>
        [HttpGet("todoItems/{id}")]
        public IActionResult GetTodoItem(long id,
                [FromServices]ITodoItemService itemService)
            => itemService.GetTodoItem(id);


        /// <summary>
        /// Get a set of todo items visible by the specified user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A set of visible todo items</returns>
        [HttpGet("todoItems/byUser/{userId}")]
        public IActionResult GetTodoItemByUser(long userId,
                [FromQuery(Name="includeunowned")]bool includeUnowned,
                [FromServices]ITodoItemService itemService)
            => itemService.GetTodoItemByUser(userId, includeUnowned);

    }
}