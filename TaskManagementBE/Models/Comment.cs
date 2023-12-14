using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementBE.Models
{
    public struct CommentViewModel
    {
        public Guid Id { get; set; }
        public Task Task { get; set; }
        public UserViewModel Creator { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string Text { get; set; }
        public CommentType Type { get; set; }
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            Task = comment.Task;
            Creator = new UserViewModel(comment.Creator);
            DateAdded = comment.DateAdded;
            ReminderDate = comment.ReminderDate;
            Text = comment.Text;
            Type = comment.Type;
            
        }
    }

    public enum CommentType
    {
        Comment,
        Reminder
    }

    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TaskId { get; set; }
        public Task Task { get; set; } = null!;

        [Required]
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;

        [Required]
        public DateTime DateAdded { get; set; } = DateTime.Now;

        public DateTime? ReminderDate { get; set; }

        [Required]
        public string Text { get; set; } = "";

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CommentType Type { get; set; }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Comment Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Comment>(json)!;
        }
    }
}
