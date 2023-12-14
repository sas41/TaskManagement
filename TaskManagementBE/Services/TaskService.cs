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
            return task;
        }

        public Models.Task Update(Models.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return task;
        }

        public Models.Task Delete(Guid id)
        {
            Models.Task task = _context.Tasks.Find(id);
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return task;
        }

        public Models.Task GetById(Guid id)
        {
            return _context.Tasks.Find(id);
        }

        public ICollection<Models.Task> GetAll(string? search)
        {
            if (search != null)
            {
                return _context.Tasks.Where(t => t.Title.Contains(search)).ToList();
            }
            return _context.Tasks.ToList();
        }
    }
}
