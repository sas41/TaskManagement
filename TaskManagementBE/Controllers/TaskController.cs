using Microsoft.AspNetCore.Mvc;
using TaskManagementBE.Services;
using TaskManagementBE.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace TaskManagementBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    [Authorize(Roles = "Admin, Manager")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public ActionResult<TaskViewModel> Create(Models.Task task)
        {
            task.CreatorId = Guid.Parse(this.User.Claims.Where(claim => claim.ValueType == "id").First().Value);
            task.Assignees = new List<User>();
            task.Comments = new List<Comment>();
            return Ok(new TaskViewModel(_taskService.Create(task)));
        }

        [HttpPut("{id}")]
        public ActionResult<TaskViewModel> Update(Guid id, Models.Task newTask)
        {
            Models.Task task = _taskService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.ValueType == "id").First().Value;
            bool isOwner = (callerId == task.CreatorId.ToString());
            bool isAdmin = this.User.IsInRole("Admin");
            if (isOwner || isAdmin)
            {
                newTask.Assignees = task.Assignees;
                newTask.Comments = task.Comments;
                return Ok(new TaskViewModel(_taskService.Update(newTask)));
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public ActionResult<TaskViewModel> Delete(Guid id)
        {
            Models.Task task = _taskService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.ValueType == "id").First().Value;
            bool isOwner = (callerId == task.CreatorId.ToString());
            bool isAdmin = this.User.IsInRole("Admin");
            if (isOwner || isAdmin)
            {
                return Ok(new TaskViewModel(_taskService.Delete(id)));
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<TaskViewModel> GetById(Guid id)
        {
            return Ok(new TaskViewModel(_taskService.GetById(id)));
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<TaskViewModel>> GetAll(string? search)
        {
            return Ok(_taskService.GetAll(search).Select(t => new TaskViewModel(t)));
        }
    }
}
