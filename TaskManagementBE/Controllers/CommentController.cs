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
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly ITaskService _taskService;


        public CommentController(ICommentService commentService, ITaskService taskService)
        {
            _commentService = commentService;
            _taskService = taskService;
        }

        [HttpPost]
        public ActionResult<CommentViewModel> Create(CommentCreateModel newComment)
        {
            Comment comment = newComment.ToComment();
            comment.CreatorId = Guid.Parse(this.User.Claims.Where(claim => claim.Type == "id").First().Value);
            return Ok(new CommentViewModel(_commentService.Create(comment)));
        }

        [HttpPut("{id}")]
        public ActionResult<CommentViewModel> Update(Guid id, CommentCreateModel newComment)
        {
            Comment comment = _commentService.GetById(id);

            string callerId = this.User.Claims.Where(claim => claim.Type == "id").First().Value;
            bool isOwner = (callerId == comment.CreatorId.ToString());
            if (isOwner)
            {
                newComment.ApplyToComment(comment);
                return Ok(new CommentViewModel(_commentService.Update(comment)));
            }
            return Unauthorized();
        }

        [HttpDelete("{id}")]
        public ActionResult<CommentViewModel> Delete(Guid id)
        {
            var comment = _commentService.GetById(id);
            var task = _taskService.GetById(comment.TaskId);

            string callerId = this.User.Claims.Where(claim => claim.Type == "id").First().Value;
            bool isOwner = (callerId == comment.CreatorId.ToString());

            bool isAdmin = this.User.IsInRole("Admin");

            bool isOwningManager = (callerId == task.CreatorId.ToString());
            if (isOwner || isAdmin || isOwningManager)
            {
                return Ok(new CommentViewModel(_commentService.Delete(id)));
            }
            return Unauthorized();
        }

        [HttpGet("{id}")]
        public ActionResult<CommentViewModel> GetById(Guid id)
        {
            return Ok(new CommentViewModel(_commentService.GetById(id)));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommentViewModel>> GetAll(string? search)
        {
            return Ok(_commentService.GetAll(search).Select(c => new CommentViewModel(c)));
        }
    }
}
