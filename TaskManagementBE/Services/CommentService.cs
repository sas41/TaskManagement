using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;
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
            var result = _context.Comments.Find(comment.Id);
            _context.Entry(result).Reference(t => t.Creator).Load();
            return result;
        }

        public Comment Update(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
            var result = _context.Comments.Find(comment.Id);
            _context.Entry(result).Reference(t => t.Creator).Load();
            return result;
        }

        public Comment Delete(Guid id)
        {
            Comment comment = _context.Comments.Find(id);
            _context.Entry(comment).Reference(t => t.Creator).Load();
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return comment;
        }

        public Comment GetById(Guid id)
        {
            Comment comment = _context.Comments.Find(id);
            _context.Entry(comment).Reference(t => t.Creator).Load();
            return comment;
        }

        public ICollection<Comment> GetAll(string? search)
        {
            List<Comment> result;
            if (search != null)
            {
                result = _context.Comments
                    .Where(c => c.Text.Contains(search))
                    .ToList();
            }
            else
            {
                result = _context.Comments.ToList();
            }

            foreach (Comment comment in result)
            {
                _context.Entry(comment).Reference(t => t.Creator).Load();
            }
            return result;
        }
    }
}
