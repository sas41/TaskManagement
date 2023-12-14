using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using TaskManagementBE.Models;

namespace TaskManagementBE
{

    public class TaskManagementContext : IdentityDbContext<User, Role, Guid>
    {
        public TaskManagementContext(DbContextOptions<TaskManagementContext> options) : base(options)
        {
        }
        public DbSet<Models.Task> Tasks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Task>()
                .HasMany(task => task.Comments)
                .WithOne(comment => comment.Task)
                .HasForeignKey(comment => comment.TaskId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Models.Task>()
                .Property(task => task.CreatorId)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<Models.Task>()
                .Property(task => task.DateAdded)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

            builder.Entity<Models.Comment>()
                .HasOne(comment => comment.Task)
                .WithMany(task => task.Comments)
                .HasForeignKey(comment => comment.TaskId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Models.Comment>()
                .Property(comment => comment.CreatorId)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            builder.Entity<Models.Comment>()
                .Property(comment => comment.DateAdded)
                .ValueGeneratedOnAddOrUpdate()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
        }
    }
}
