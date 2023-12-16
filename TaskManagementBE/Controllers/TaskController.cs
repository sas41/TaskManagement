using Microsoft.AspNetCore.Mvc;
using TaskManagementBE.Services;
using TaskManagementBE.Models;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TaskManagementBE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly UserManager<User> _userManager;

        public TaskController(UserManager<User> userManager, ITaskService taskService)
        {
            _taskService = taskService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<TaskViewModel>> Create(TaskCreateModel newTask)
        {
            Models.Task task = newTask.ToTask();
            string id = (this.User.Claims.Where(claim => claim.Type == "id").First().Value);
            task.CreatorId = Guid.Parse(id);
            var createdTask = _taskService.Create(task);
            return Ok(new TaskViewModel(createdTask));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult<TaskViewModel> Update(Guid id, TaskCreateModel newTask)
        {
            Models.Task task = _taskService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.Type == "id").First().Value;
            bool isOwner = (callerId == task.CreatorId.ToString());
            bool isAdmin = this.User.IsInRole("Admin");
            if (isOwner || isAdmin)
            {
                newTask.ApplyToTask(task);
                return Ok(new TaskViewModel(_taskService.Update(task)));
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult<TaskViewModel> Delete(Guid id)
        {
            Models.Task task = _taskService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.Type == "id").First().Value;
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
            var tasks = _taskService.GetAll(search).Select(t => new TaskViewModel(t)).ToList();
            return Ok(tasks);
        }

        [HttpPost("Assign/{id}")]
        [Authorize]
        public async Task<ActionResult<TaskViewModel>> Assign(Guid id, [FromBody]ICollection<Guid> userIds)
        {
            Models.Task task = _taskService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.Type == "id").First().Value;
            bool isOwner = (callerId == task.CreatorId.ToString());
            bool isAdmin = this.User.IsInRole("Admin");
            if (isOwner || isAdmin)
            {
                task.Assignees.Clear();
                foreach (Guid userId in userIds)
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    if (user != null) task.Assignees.Add(user);
                }
                _taskService.Update(task);
                return Ok(new TaskViewModel(task));
            }
            return Unauthorized();
        }
    }
}
