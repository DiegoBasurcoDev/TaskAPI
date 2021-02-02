using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using TaskAPI;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("Task")]
    public class TaskController : ControllerBase
    {
        private readonly TaskDBContext _taskDbContext;
        public TaskController(TaskDBContext taskDbContext)
        {
            _taskDbContext = taskDbContext;
        }

        [HttpGet]
        public List<Task> ListAll()
        {
            var task = _taskDbContext.Task.ToList();
            return task;
        }

        [HttpGet]
        [Route("{taskId}")]
        public List<Task> List(int taskId){
            var task = _taskDbContext.Task.Where(t => t.ID == taskId).ToList();
            return task;
        }

        [HttpPost]
        public ActionResult Register(Task t)
        {
            try
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                IList<Claim> claim = identity.Claims.ToList();

                var task = new Task{
                    Name = t.Name,
                    Content = t.Content,
                    AuthorID = Int32.Parse(claim[0].Value)
                };
                _taskDbContext.Task.Add(task);
                _taskDbContext.SaveChanges();
                return Ok(new
                {
                    Status = 1,
                    Message = "Task registered",
                    Task = task
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "Failed to register task",
                    Detail = ex.Message
                });
            }
        }

        [HttpPut]
        public ActionResult Update(Task t)
        {
            try
            {
                _taskDbContext.Entry(t).State = EntityState.Modified;
                _taskDbContext.SaveChanges();
                return Ok(new {
                    Status = 1,
                    Message = "Task updated"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "Failed to update task",
                    Detail = ex.Message
                });
            }
        }

        [HttpDelete]
        public ActionResult Delete(Task t){
            try
            {
                _taskDbContext.Task.Remove(t);
                _taskDbContext.SaveChanges();
                return Ok(new {
                    Status = 1,
                    Message = "Task deleted"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Status = 0,
                    Message = "Failed to delete task",
                    Detail = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("ListAuthor")]
        public List<Task> ListAuthorTask(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            
            var task = _taskDbContext.Task.Where(t => t.AuthorID == Int32.Parse(claim[0].Value)).ToList();
            return task;
        }
    }
}