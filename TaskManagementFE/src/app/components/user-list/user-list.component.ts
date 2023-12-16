import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { TaskService } from '../../services/task/task.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [NgFor],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  users: any[] = [];
  constructor(private taskService: TaskService)
  {
    this.taskService.getPossibleAssignees().then((users) => {
      this.users = users || [];
    });
  }
}
