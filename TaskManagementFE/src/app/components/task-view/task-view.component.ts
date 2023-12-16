import { Component, Input } from '@angular/core';
import { KeyValuePipe, NgFor, NgIf, ViewportScroller, formatDate } from '@angular/common';
import {ActivatedRoute, Router} from "@angular/router";
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth/auth.service';
import { TaskService } from '../../services/task/task.service';
import { CommentService } from '../../services/comment/comment.service';
import { Comment } from '../../models/comment';
import { Task } from '../../models/task';

const defaultDueInDays = 1;
const defaultDueIn = 1000*60*60*24*defaultDueInDays;

@Component({
  selector: 'app-task-view',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf, KeyValuePipe],
  templateUrl: './task-view.component.html',
  styleUrl: './task-view.component.css'
})

export class TaskViewComponent {
  task: Task = Task.Placeholder;
  id: string = '';

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
          this.generateAssignmentModel();
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
    return formatDate(date ?? '0000-00-00', 'yyyy-MM-dd', 'en');
  }

  displayCommentsSection()
  {
    return this.task.comments && this.task.comments.length > 0;
  }

  //////////////////////////////////
  // Comment Posting and Editing. //
  //////////////////////////////////
  public commentTypes = Comment.CommentTypes;
  public commentModel:Comment = new Comment('', 'Comment', '', null, null, formatDate(new Date(new Date().getTime() + defaultDueIn), 'yyyy-MM-dd', 'en'));
  commentForm = this.formBuilder.group(
    this.commentModel
  );
  public commentIsUpdate = false;
  public commentError = false;
  private commentSubmitted = false;

  editComment(comment: Comment)
  {
    this.commentIsUpdate = true;
    this.commentModel = comment;
    this.commentForm.patchValue(comment);
    this.scroller.scrollToAnchor("commentForm");
  }

  cancelEditComment()
  {
    this.commentIsUpdate = false;
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
    this.commentError = false;
    this.commentSubmitted = false;

    if (this.commentForm.valid)
    {
      try
      {
        var result;
        var data = this.commentForm.value as Comment;
        data.taskId = this.id;
        if (this.commentIsUpdate && data.id)
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
        this.commentError = true;
      }
    }
    else
    {
      this.commentSubmitted = true;
    }
  }

  /////////////////////////
  // Assignment Editing. //
  /////////////////////////

  public assignmentProspects: any[] = [];
  public assignmentModel: string[] = [];

  public assignForm = this.formBuilder.group({
    userList: this.formBuilder.array([]),
  });

  public assignmentError = false;
  private assignmentSubmitted = false;

  generateAssignmentModel()
  {
    this.assignmentModel = this.task.assignees?.map((assignee) => assignee.id) ?? [];
  }

  showAssignment()
  {

    this.taskService.getPossibleAssignees().then((users) => {
      if (users)
      {
        this.assignmentProspects = users.map((user) => { return { ...user, assignedToThis: this.task.assignees?.map(a => a.id).includes(user.id) } });
      }
      
      
      this.generateAssignmentModel();
      const assignment = document.getElementById("assignment");
      if (assignment)
      {
        assignment.style.display = "block";
      }
    });
  }

  closeAssignment()
  {
    const assignment = document.getElementById("assignment");
    if (assignment)
    {
      assignment.style.display = "none";
    }
    this.generateAssignmentModel();
  }

  markAssignment(event: any)
  {
    const checkArray: FormArray = this.assignForm.get('userList') as FormArray;
    if (event.target.checked)
    {
      checkArray.push(new FormControl(event.target.value));
    }
    else
    {
      let i: number = 0;
      checkArray.controls.forEach((item) => {
        if (item.value == event.target.value) {
          checkArray.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  toFormControl(control: any) : FormControl
  {
    return control as FormControl;
  }

  async onAssignmentSubmit() {
    this.assignmentError = false;
    this.assignmentSubmitted = false;
    try
    {
      var data = this.assignForm.value.userList as string[];
      var result = await this.taskService.assignToTask(this.id, data);
      if (result?.id)
      {
        window.location.reload();
      }
    }
    catch (err)
    {
      console.log(err);
      this.assignmentError = true;
    }
  }
}
