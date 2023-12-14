using TaskManagementBE.Models;

namespace TaskManagementBE.Services
{
    public interface ITaskService
    {
        Models.Task Create(Models.Task task);
        Models.Task Update(Models.Task task);
        Models.Task Delete(Guid id);
        Models.Task GetById(Guid id);
        ICollection<Models.Task> GetAll(string? search);
    }
}
