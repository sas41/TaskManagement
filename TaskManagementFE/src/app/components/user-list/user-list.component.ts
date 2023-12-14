import { Component } from '@angular/core';
import { NgFor } from '@angular/common';
import { ServerService } from '../../services/server/server.service';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [NgFor],
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css'
})
export class UserListComponent {
  users: any[] = [];
  constructor(private server: ServerService)
  {
    this.server.request('GET', '/api/User', {}).subscribe((data: any) => {
      this.users = data;
    });
  }
}
