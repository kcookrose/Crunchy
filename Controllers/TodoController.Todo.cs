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
            return _context.TodoItems.ToList();
        }


        [HttpGet("todo/{Tid}", Name = "GetTodo")]
        public IActionResult GetTodoById(long Tid) {
            var item = _context.TodoItems.Find(Tid);
            if (item == null) return NotFound();
            return Ok(item);
        }


        [HttpGet("todo/byUser/{userId}")]
        public object GetTodoByUserId(long userId) {
            return  _context.TodoItems
                    .Include(todoItem => todoItem.Assignee)
                    .Include(todoItem => todoItem.OwnerTodoItem)
                    .Include(todoItem => todoItem.Status)
                    .ToList();
        }
   }
}