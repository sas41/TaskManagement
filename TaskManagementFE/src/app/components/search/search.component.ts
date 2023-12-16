import { Component } from '@angular/core';
import { NgFor, formatDate, NgIf } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { TaskService } from '../../services/task/task.service';
import { CommentService } from '../../services/comment/comment.service';
import { Task } from '../../models/task';
import { Comment } from '../../models/comment';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './search.component.html',
  styleUrl: './search.component.css'
})
export class SearchComponent {
  tasks: Task[] = [];
  comments: Comment[] = [];
  query: string | undefined;

  constructor(private route: ActivatedRoute, public router: Router, public auth: AuthService, private taskService: TaskService, private commentsService: CommentService)
  {
    // lastValueFrom(this.route.queryParams).then((params) => {
    //   this.query = params['query'];

    //   if(this.query)
    //   {
    //     taskService.getTasks(this.query).then((tasks) => {
    //       this.tasks = tasks ?? [];
    //     });
    //     commentsService.getComments(this.query).then((comments) => {
    //       this.comments = comments ?? [];
    //     });
    //   }
    // });
    this.route.queryParams.subscribe((params) => {
      this.query = params['query'];

      if(this.query)
      {
        taskService.getTasks(this.query).then((tasks) => {
          this.tasks = tasks ?? [];
        });
        commentsService.getComments(this.query).then((comments) => {
          this.comments = comments ?? [];
        });
      }
    });
  }
  
  dateDisplay(date: string | undefined) {
    return formatDate(date ?? '', 'yyyy-MM-dd', 'en');
  }

  navigateToTask(id: string) {
    this.router.navigate([`/task/view/${id}`]);
  }
}
