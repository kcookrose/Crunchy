using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        /// Get all todo items
        /// NOTE: Should probably be removed in relase
        /// </summary>
        [HttpGet("todoItems")]
        public IActionResult GetAllTodoItems() {
            using (var context = new TodoContext()) {
                var res = context.TodoItems
                    .Include(todoItem => todoItem.Project)
                    .Include(todoItem => todoItem.Assignee)
                    .Select(todo => GetShortModel(todo))
                    .ToArray();
                return Ok(res);
            }
        }


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
                    EnsureDeepLoaded(context.Entry(todoItem));
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
            if (userId < 1) return NotFound();
            using (var context = new TodoContext()) {
                var filteredItems = context.TodoItems
                    .Include(todoItem => todoItem.Assignee)
                    .Include(todoItem => todoItem.Project)
                    .ToArray();
                var formattedItems = filteredItems
                    .Where(todoItem => todoItem.AssigneeId == -1 ||
                        todoItem.AssigneeId == userId)
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
            using (var context = new TodoContext()) {
                return new {
                    Tid = todoItem.Tid,
                    Name = todoItem.Name,
                    Tags = todoItem.Tags,
                    ProjectId = todoItem.ProjectId,
                    AssigneeId = todoItem.AssigneeId
                };
            }
        }


        /// <summary>
        /// Get a detailed representation of the todo item
        /// </summary>
        public object GetDetailedModel(TodoItem todoItem) {
            using (var context = new TodoContext()) {
                var entry = context.Entry(todoItem);
                if (entry.State == EntityState.Detached)
                    entry = context.Attach(todoItem);
                foreach (var navItem in entry.Navigations)
                    navItem.Load();
                foreach (var collection in entry.Collections)
                    collection.Load();
            }
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