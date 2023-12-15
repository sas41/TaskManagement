using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementBE.Models
{
    public enum CommentType
    {
        Comment,
        Reminder
    }
    public struct CommentCreateModel
    {
        public Guid? Id { get; set; }
        [Required]
        public Guid TaskId { get; set; }
        [Required]
        public CommentType Type { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime? ReminderDate { get; set; }
        public Comment ToComment()
        {
            return new Comment
            {
                TaskId = this.TaskId,
                Type = this.Type,
                Text = this.Text,
                ReminderDate = this.ReminderDate
            };
        }
        public void ApplyToComment(Comment comment)
        {
            comment.Type = this.Type;
            comment.Text = this.Text;
            comment.ReminderDate = this.ReminderDate;
        }
    }
    public struct CommentViewModel
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public UserViewModel Creator { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? ReminderDate { get; set; }
        public string Text { get; set; }
        public CommentType Type { get; set; }
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id;
            TaskId = comment.TaskId;
            Creator = new UserViewModel(comment.Creator);
            DateAdded = comment.DateAdded;
            ReminderDate = comment.ReminderDate;
            Text = comment.Text;
            Type = comment.Type;
            
        }
    }

    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid TaskId { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
        public User Creator { get; set; } = null!;


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateAdded { get; set; }

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
