import { Component, Input } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { NgFor, ViewportScroller, formatDate } from '@angular/common';
import { NgIf } from '@angular/common';
import { TaskService } from '../../services/task/task.service';
import { Task } from '../../models/task';
import {ActivatedRoute, Router} from "@angular/router";
import { AuthService } from '../../services/auth/auth.service';
import { Comment } from '../../models/comment';
import { CommentService } from '../../services/comment/comment.service';
import { DomElementSchemaRegistry } from '@angular/compiler';

const defaultDueInDays = 1;
const defaultDueIn = 1000*60*60*24*defaultDueInDays;

@Component({
  selector: 'app-task-view',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf],
  templateUrl: './task-view.component.html',
  styleUrl: './task-view.component.css'
})

export class TaskViewComponent {
  
  public commentTypes = Comment.CommentTypes;
  public commentModel:Comment = new Comment('', 'Comment', '', null, null, formatDate(new Date(new Date().getTime() + defaultDueIn), 'yyyy-MM-dd', 'en'));
  commentForm = this.formBuilder.group(
    this.commentModel
  );
  public isUpdate = false;
  public error = false;
  private submitted = false;

  task: Task = new Task('', '', '', 'New', 'Task');
  id: string = '';

  get assignedToListFlat()
  {
    return this.task.assignees?.map((assignee) => assignee.username).join(', ');
  }

  get assignedToAnyone()
  {
    return this.task.assignees?.length ?? 0 > 0;
  }

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private scroller: ViewportScroller,
    public auth: AuthService,
    private taskService: TaskService,
    private commentService: CommentService)
  {
    this.route.params.subscribe(params => {
      this.taskService.getTask(params['id']).then((task) => {
        if(task)
        {
          this.task = task;
          this.id = params['id'];
        }
        else
        {
          this.router.navigate(['/tasks']);
        }
      });
    });
  }

  editTask() {
    this.router.navigate([`/task/edit/${this.id}`]);
  }

  async deleteTask() {
    var result = await this.taskService.deleteTask(this.id);
    this.router.navigate(['/tasks']);
  }

  dateDisplay(date: string | undefined) {
    return formatDate(date ?? '', 'yyyy-MM-dd', 'en');
  }

  displayCommentsSection()
  {
    return this.task.comments && this.task.comments.length > 0;
  }

  editComment(comment: Comment)
  {
    this.isUpdate = true;
    this.commentModel = comment;
    this.commentForm.patchValue(comment);
    this.scroller.scrollToAnchor("commentForm");
  }

  cancelEditComment()
  {
    this.isUpdate = false;
    this.commentModel = new Comment('', 'Comment', '', null, null, formatDate(new Date(new Date().getTime() + defaultDueIn), 'yyyy-MM-dd', 'en'));
    this.commentForm.patchValue(this.commentModel);
  }

  async deleteComment(comment: Comment)
  {
    if (comment.id)
    {
      var result = await this.commentService.deleteComment(comment.id);
      if (result?.id)
      {
        window.location.reload();
      }
    }
  }

  async onCommentSubmit() {
    this.error = false;
    this.submitted = false;

    if (this.commentForm.valid)
    {
      try
      {
        var result;
        var data = this.commentForm.value as Comment;
        data.taskId = this.id;
        if (this.isUpdate && data.id)
        {
          result = await this.commentService.updateComment(data, data.id);
        } 
        else
        {
          result = await this.commentService.createComment(data);
        }
        if (result?.id)
        {
          window.location.reload();
        }
      }
      catch (err)
      {
        console.log(err);
        this.error = true;
      }
    }
    else
    {
      this.submitted = true;
    }
  }

  editAssigned()
  {
    
  }
}
