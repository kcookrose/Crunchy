using System.Threading.Tasks;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Crunchy.Services.Interfaces;
using Crunchy.Models;


namespace Crunchy.Services {

    public class TodoItemService : CrunchyService, ITodoItemService {

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


        public IActionResult GetTodoItem(long id) {
            using (var context = new TodoContext()) {
                var todoItem = context.TodoItems.Find(id);
                if (todoItem != null) {
                    context.EnsureDeepLoaded(todoItem);
                    return Ok(GetDetailedModel(todoItem));
                }
            }
            return new NotFoundResult();
        }


        public IActionResult GetTodoItemByUser(long userId, bool includeUnowned) {
            if (userId < 1) return NotFound();
            using (var context = new TodoContext()) {
                var filteredItems = context.TodoItems
                    .Include(todoItem => todoItem.Assignee)
                    .Include(todoItem => todoItem.Project)
                    .ToArray();
                var formattedItems = filteredItems
                    .Where(todoItem =>
                            (includeUnowned && todoItem.AssigneeId == -1) ||
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