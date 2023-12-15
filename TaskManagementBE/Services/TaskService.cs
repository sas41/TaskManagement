using Microsoft.EntityFrameworkCore;
using TaskManagementBE.Models;

namespace TaskManagementBE.Services
{
    public class TaskService : ITaskService
    {
        private TaskManagementContext _context;
        public TaskService(TaskManagementContext db)
        {
            _context = db;
        }

        public Models.Task Create(Models.Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            var result = _context.Tasks.Find(task.Id);
            _context.Entry(result).Reference(t => t.Creator).Load();
            _context.Entry(result).Collection(t => t.Assignees).Load();
            _context.Entry(result).Collection(t => t.Comments).Load();
            return result;
        }

        public Models.Task Update(Models.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            var result = _context.Tasks.Find(task.Id);
            _context.Entry(result).Reference(t => t.Creator).Load();
            _context.Entry(result).Collection(t => t.Assignees).Load();
            _context.Entry(result).Collection(t => t.Comments).Load();
            return result;
        }

        public Models.Task Delete(Guid id)
        {
            Models.Task task = _context.Tasks.Find(id);
            _context.Entry(task).Reference(t => t.Creator).Load();
            _context.Entry(task).Collection(t => t.Assignees).Load();
            _context.Entry(task).Collection(t => t.Comments).Load();
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return task;
        }

        public Models.Task GetById(Guid id)
        {
            Models.Task task = _context.Tasks.Find(id);
            _context.Entry(task).Reference(t => t.Creator).Load();
            _context.Entry(task).Collection(t => t.Assignees).Load();
            _context.Entry(task).Collection(t => t.Comments).Load();
            return task;
        }

        public ICollection<Models.Task> GetAll(string? search)
        {
            List<Models.Task> result;
            if (search != null)
            {
                result = _context.Tasks
                    .Where(t => t.Title.Contains(search))
                    .ToList();
            }
            else
            {
                result = _context.Tasks.ToList();
            }

            foreach (var task in result)
            {
                _context.Entry(task).Reference(t => t.Creator).Load();
                _context.Entry(task).Collection(t => t.Assignees).Load();
                _context.Entry(task).Collection(t => t.Comments).Load();
            }
            return result;
        }
    }
}
