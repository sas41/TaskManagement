using Microsoft.EntityFrameworkCore.Storage;
using TaskManagementBE.Models;

namespace TaskManagementBE.Services
{
    public class CommentService : ICommentService
    {
        private TaskManagementContext _context;
        public CommentService(TaskManagementContext db)
        {
            _context = db;
        }

        public Comment Create(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment Delete(Guid id)
        {
            Comment comment = _context.Comments.Find(id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment GetById(Guid id)
        {
            return _context.Comments.Find(id);
        }

        public ICollection<Comment> GetAll(string? search)
        {
            if (search != null)
            {
                return _context.Comments.Where(c => c.Text.Contains(search)).ToList();
            }
            return _context.Comments.ToList();
        }
    }
}
