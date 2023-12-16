import { Component } from '@angular/core';
import { NgIf, NgFor, formatDate } from '@angular/common';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth/auth.service';
import { TaskService } from '../../services/task/task.service';
import { Task } from '../../models/task';

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
    });
  }
  
  dateDisplay(date: string | undefined) {
    return formatDate(date ?? '', 'yyyy-MM-dd', 'en');
  }

  navigateToTask(id: string) {
    this.router.navigate([`/task/view/${id}`]);
  }
}
