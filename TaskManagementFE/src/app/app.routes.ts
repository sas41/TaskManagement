import { Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { LoginComponent } from './components/login/login.component';
import { DocComponent } from './components/doc/doc.component';
import { UserListComponent } from './components/user-list/user-list.component';

export const routes: Routes = [
    { path: 'tasks', component: TaskListComponent },
    { path: 'login', component: LoginComponent },
    { path: 'help', component: DocComponent },
    { path: 'users', component: UserListComponent },
];
