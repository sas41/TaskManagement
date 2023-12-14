import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { ServerService } from '../../services/server/server.service';

@Component({
  selector: 'app-task-list',
  standalone: true,
  imports: [NgFor],
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {
  tasks: any[] = [];
  constructor(private server: ServerService)
  {
    this.server.request('GET', '/api/Task').subscribe((data: any) => {
      this.tasks = data;
    });
  }
}
