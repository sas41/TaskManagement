using TaskManagementBE.Models;

namespace TaskManagementBE.Services
{
    public interface ICommentService
    {
        Comment Create(Comment comment);
        Comment Update(Comment comment);
        Comment Delete(Guid id);
        Comment GetById(Guid id);
        ICollection<Comment> GetAll(string? search);
    }
}
