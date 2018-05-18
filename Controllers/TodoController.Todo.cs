using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Crunchy.Models;
using System.Linq;

/*
 * This file should contain controller elements dedicated to working with
 * TodoItems
 */

namespace Crunchy.Controllers {

    public partial class TodoController : ControllerBase {

        [HttpGet("todo")]
        public List<TodoItem> GetAllTodo() {
            using (var context = new TodoContext()) {
                return context.TodoItems.ToList();
            }
        }


        [HttpGet("todo/{Tid}", Name = "GetTodo")]
        public IActionResult GetTodoById(long Tid) {
            using (var context = new TodoContext()) {
                var item = context.TodoItems.Find(Tid);
                if (item == null) return NotFound();
                return Ok(item);
            }
        }


        [HttpGet("todo/byUser/{userId}")]
        public object GetTodoByUserId(long userId) {
            using (var context = new TodoContext()) {
                return  context.TodoItems
                    .Include(todoItem => todoItem.Assignee)
                    .Include(todoItem => todoItem.OwnerTodoItem)
                    .Include(todoItem => todoItem.Status)
                    .ToList();
            }
        }
   }
}