using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Crunchy.Models;


/*
 * This file should contain controller elements dedicated to working with
 * Projects
 */

namespace Crunchy.Controllers {

    public partial class TodoController : ControllerBase {


        /// <summary>
        /// Get a todoItem by its ID.
        /// </summary>
        /// <param name="id">The ID of the desired todoitem</param>
        /// <returns>The requested todo item</returns>
        [HttpGet("todoItems/{id}")]
        public IActionResult GetTodoItem(long id) {
            using (var context = new TodoContext()) {
                var todoItem = context.TodoItems.Find(id);
                if (todoItem != null) {
                    return Ok(GetDetailedModel(todoItem));
                }
            }
            return NotFound();
        }


        /// <summary>
        /// Get a set of todo items visible by the specified user
        /// </summary>
        /// <param name="userId">The ID of the user</param>
        /// <returns>A set of visible todo items</returns>
        [HttpGet("todoItems/byUser/{userId}")]
        public IActionResult GetTodoItemByUser(long userId) {
            using (var context = new TodoContext()) {
                var filteredItems = context.TodoItems
                    .Where(todoItem => (todoItem.Assignee == null || 
                                        todoItem.Assignee.Uid == userId));
                var formattedItems = filteredItems
                    .Select(todoItem => GetShortModel(todoItem))
                    .ToArray();
                if (formattedItems.Length > 0)
                    return Ok(formattedItems);
            }
            return NotFound();
        }


        /// <summary>
        /// Get a shortened representation of the todo item
        /// </summary>
        public object GetShortModel(TodoItem todoItem) {
            return new {
                Tid = todoItem.Tid,
                Name = todoItem.Name,
                Tags = todoItem.Tags,
                ProjectId = todoItem.Project.Pid,
                AssigneeId = todoItem.AssigneeId
            };
        }


        /// <summary>
        /// Get a detailed representation of the todo item
        /// </summary>
        public object GetDetailedModel(TodoItem todoItem) {
            return new {
                Tid = todoItem.Tid,
                Name = todoItem.Name,
                StartDateTime = todoItem.StartDateTime,
                DueDateTime = todoItem.DueDateTime,
                Tags = todoItem.Tags,
                EstimatedTime = todoItem.EstimatedTime,
                FileIds = todoItem.FileIds,
                OwnerTodoItemId = todoItem.OwnerTodoItemId,
                ProjectId = todoItem.ProjectId,
                StatusId = todoItem.StatusId,
                AssigneeId = todoItem.AssigneeId,
                RequiredItemIds = todoItem.RequiredItemIds
            };
        }
    }
}