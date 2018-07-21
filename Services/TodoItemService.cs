using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

using Crunchy.Services.Interfaces;
using Crunchy.Models;

using Newtonsoft.Json;

namespace Crunchy.Services {

    public class TodoItemService : CrunchyService, ITodoItemService {

        public ILoggerService Log { get; set; }

        public TodoItemService(ILoggerService logger) {
            Log = logger;
        }

        public IActionResult GetAllTodoItems() {
            using (var context = new TodoContext()) {
                var res = context.TodoItems
                    .Select(todo => GetShortModel(context, todo))
                    .ToArray();
                return Ok(res);
            }
        }


        public IActionResult GetTodoItem(long id) {
            using (var context = new TodoContext()) {
                var todoItem = context.TodoItems.Find(id);
                if (todoItem != null) {
                    var ent = context.Entry(todoItem);
                    context.EnsureDeepLoaded(ent);
                    return Ok(GetDetailedModel(context, todoItem));
                }
            }
            return new NotFoundResult();
        }


        public IActionResult CreateTodoItem(string json) {
            Log.Client("Client Create TodoItem");
            using (var context = new TodoContext()) {
                TodoItem newItem = TodoItemFromJson(context, json);
                if (newItem == null) return BadRequest();
                context.ChangeTracker.TrackGraph(newItem, node => node.Entry.State = (node.Entry.IsKeySet ? EntityState.Unchanged : EntityState.Added));
                try {
                    context.SaveChanges();
                } catch (DbUpdateException exOutter) {
                    SqliteException ex = exOutter.InnerException as SqliteException;
                    Log.Error("SQL Error: " + ex.SqliteErrorCode.ToString() + ": " + (ex.SqliteExtendedErrorCode/256).ToString());
                    return BadRequest();
                }
                Log.Client("Success");
                return CreatedAtRoute("GetTodoItem", new { id = newItem.Tid }, GetDetailedModel(context, newItem));
            }
        }


        public IActionResult UpdateTodoItem(long id, string json) {
            Log.Client("Update TodoItem with ID: " + id);
            using (var context = new TodoContext()) {
                TodoItem newItem = TodoItemFromJson(context, json);
                if (newItem == null) return BadRequest();
                TodoItem oldItem = context.TodoItems.Find(id);
                if (oldItem == null) return BadRequest();
                oldItem.Name = newItem.Name;
                oldItem.AssigneeId = newItem.AssigneeId;
                oldItem.Assignee = newItem.Assignee;                    
                oldItem.DueDateTime = newItem.DueDateTime;
                oldItem.EstimatedTime = newItem.EstimatedTime;
                oldItem.OwnerTodoItem = newItem.OwnerTodoItem;
                oldItem.OwnerTodoItemId = newItem.OwnerTodoItemId;
                oldItem.Project = newItem.Project;
                oldItem.ProjectId = newItem.ProjectId;
                oldItem.RequiredItems.Clear();
                foreach (var reqItem in newItem.RequiredItems)
                    oldItem.RequiredItems.Add(reqItem);
                oldItem.StartDateTime = newItem.StartDateTime;
                oldItem.Status = newItem.Status;
                oldItem.StatusId = newItem.StatusId;
                oldItem.Tags = newItem.Tags;
                Log.Client("Success");
                context.SaveChanges();
                return NoContent();
            }
        }


        public TodoItem TodoItemFromJson(TodoContext context, string json) {
            var parsedTodoItem = new {
                Tid = 0L,
                Name = "",
                StartDataTime = (DateTime?)null,
                DueDateTime = (DateTime?)null,
                Tags = (string)null,
                EstimatedTime = (TimeSpan?)null,
                OwnerTodoItemId = (long?)null,
                ProjectId = 0L,
                StatusId = 0L,
                AssigneeId = (long?)null,
                RequiredItemIds = new long[0]
            };
            try {
                parsedTodoItem = JsonConvert.DeserializeAnonymousType(json, parsedTodoItem);
            } catch (JsonException ex) {
                System.Console.WriteLine("Exception while parsing JSON: " + ex.Message);
                return null;
            }
            TodoItem res = new TodoItem();
            res.Tid = parsedTodoItem.Tid;
            res.Name = parsedTodoItem.Name;
            if (String.IsNullOrEmpty(res.Name)) {
                Log.Client("Empty name");
                return null;
            }
            res.Project = context.Projects.Find(parsedTodoItem.ProjectId);
            res.ProjectId = parsedTodoItem.ProjectId;
            if (!ParseValidate(parsedTodoItem.ProjectId, res.Project, "Project"))
                return null;
            res.Status = context.Statuses.Find(parsedTodoItem.StatusId);
            res.StatusId = parsedTodoItem.StatusId;
            if (res.Status == null) {
                Log.Client("Missing status");
                return null;
            }
            if (!ParseValidate(parsedTodoItem.StatusId, res.Status, "Status"))
                return null;
            res.StartDateTime = parsedTodoItem.StartDataTime;
            res.DueDateTime = parsedTodoItem.DueDateTime;
            res.Tags = parsedTodoItem.Tags;
            res.EstimatedTime = parsedTodoItem.EstimatedTime;
            if (parsedTodoItem.OwnerTodoItemId.HasValue) {
                res.OwnerTodoItem = context.TodoItems.Find(parsedTodoItem.OwnerTodoItemId.Value);
                res.OwnerTodoItemId = parsedTodoItem.OwnerTodoItemId;
                if (!ParseValidate(parsedTodoItem.OwnerTodoItemId.Value, res.OwnerTodoItem, "Owner Item"))
                    return null;
            }
            if (parsedTodoItem.AssigneeId.HasValue) {
                res.Assignee = context.Users.Find(parsedTodoItem.AssigneeId.Value);
                res.AssigneeId = parsedTodoItem.AssigneeId;
                if (!ParseValidate(parsedTodoItem.AssigneeId.Value, res.Assignee, "Assignee"))
                    return null;
            }
            res.RequiredItems = new List<TodoItem>();
            foreach (long itemId in parsedTodoItem.RequiredItemIds) {
                TodoItem otherItem = context.TodoItems.Find(itemId);
                if (otherItem == null) {
                    Log.Client("Invalid Required Item");
                    return null;
                }
                res.RequiredItems.Add(otherItem);
            }
            return res;
        }


        public bool ParseValidate(long id, object value, string fieldname) {
            if (id > 0 && value == null) {
                Log.Client("Invalid " + fieldname);
                return false;
            }
            return true;
        }


        /* // TODO: Re-add removed interface
        public IActionResult GetTodoItemByUser(long userId, bool includeUnowned) { 
            if (userId < 1) return NotFound();
            using (var context = new TodoContext()) {
                var filteredItems = context.TodoItems
                    .ToArray();
                var formattedItems = filteredItems
                    .Where(todoItem =>
                            (includeUnowned && todoItem.AssigneeId == -1) ||
                            todoItem.AssigneeId == userId)
                    .Select(todoItem => GetShortModel(context, todoItem))
                    .ToArray();
                if (formattedItems.Length > 0)
                    return Ok(formattedItems);
            }
            return NotFound();
        }*/


        /// <summary>
        /// Get a shortened representation of the todo item
        /// </summary>
        public object GetShortModel(TodoContext context, TodoItem todoItem) {
            return new {
                Tid = todoItem.Tid,
                Name = todoItem.Name,
                Tags = todoItem.Tags,
                ProjectId = todoItem.ProjectId,
                AssigneeId = todoItem.AssigneeId
            };
        }


        /// <summary>
        /// Get a detailed representation of the todo item
        /// </summary>
        public object GetDetailedModel(TodoContext context, TodoItem todoItem) {
            return new {
                Tid = todoItem.Tid,
                Name = todoItem.Name,
                StartDateTime = todoItem.StartDateTime,
                DueDateTime = todoItem.DueDateTime,
                Tags = todoItem.Tags,
                EstimatedTime = todoItem.EstimatedTime,
                //FileIds = todoItem.FileIds,
                OwnerTodoItemId = todoItem.OwnerTodoItemId,
                ProjectId = todoItem.ProjectId,
                StatusId = todoItem.StatusId,
                AssigneeId = todoItem.AssigneeId,
                RequiredItemIds = todoItem.RequiredItemIds
            };
        }
    }
}