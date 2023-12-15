import { Component } from '@angular/core';
import { NgFor, formatDate } from '@angular/common';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { TaskService } from '../../services/task/task.service';
import { Task } from '../../models/task';
import { Router } from '@angular/router';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [NgFor, NgIf],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {
  tasks: Task[] = [];
  constructor(public router: Router, public auth: AuthService, private taskService: TaskService)
  {
    taskService.getTasks().then((tasks) => {
      this.tasks = tasks ?? [];
      for (let task of this.tasks)
      {
        task.assignees = task.assignees?.map(assignee => assignee.name);
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
