using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagementBE.Models
{
    public enum TaskStatus
    {
        New,
        InProgress,
        Done
    }

    public enum TaskType
    {
        Task,
        Bug
    }

    public struct TaskCreateModel
    {
        public Guid? Id { get; set; }
        [Required]
        public DateTime RequiredBy { get; set; }
        [Required]
        public TaskStatus Status { get; set; }
        [Required]
        public TaskType Type { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public Task ToTask()
        {
            return new Task
            {
                RequiredBy = this.RequiredBy,
                Status = this.Status,
                Type = this.Type,
                Title = this.Title,
                Description = this.Description
            };
        }

        public void ApplyToTask(Task task)
        {
            task.RequiredBy = this.RequiredBy;
            task.Status = this.Status;
            task.Type = this.Type;
            task.Title = this.Title;
            task.Description = this.Description;
        }
    }

    public struct TaskViewModel
    {
        public Guid Id { get; set; }
        public UserViewModel Creator { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime RequiredBy { get; set; }
        public DateTime NextActionDate { get; set; }
        public TaskStatus Status { get; set; }
        public TaskType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<UserViewModel>? Assignees { get; set; }
        public ICollection<CommentViewModel>? Comments { get; set; }
        public TaskViewModel(Task task)
        {
            Id = task.Id;
            Creator = new UserViewModel(task.Creator);
            DateAdded = task.DateAdded;
            RequiredBy = task.RequiredBy;
            NextActionDate = task.NextActionDate;
            Status = task.Status;
            Type = task.Type;
            Title = task.Title;
            Description = task.Description;
            Assignees = task.Assignees?.Select(a => new UserViewModel(a)).ToList();
            Comments = task.Comments?.Select(c => new CommentViewModel(c)).ToList();
        }
    }

    public class Task
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CreatorId { get; set; }
        public User? Creator { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime DateAdded { get; set; }

        [Required]
        public DateTime RequiredBy { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatus Status { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskType Type { get; set; }

        [Required]
        [MaxLength(128)]
        public string Title { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        public ICollection<User>? Assignees { get; set; } = new List<User>();

        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();

        public DateTime NextActionDate
        {
            get
            {

                return this.Comments?
                    .Where(c => c.Type == CommentType.Reminder)
                    .OrderBy(c => c.ReminderDate)
                    .Select(c => c.ReminderDate)
                    .DefaultIfEmpty(RequiredBy)
                    .First() ?? RequiredBy;
            }
        }

        public string Serialize()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Task Deserialize(string json)
        {
            return JsonSerializer.Deserialize<Task>(json)!;
        }
    }
}
