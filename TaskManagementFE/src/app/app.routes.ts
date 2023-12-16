import { Routes } from '@angular/router';
import { TaskListComponent } from './components/task-list/task-list.component';
import { LoginComponent } from './components/login/login.component';
import { HelpComponent } from './components/help/help.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { TaskViewComponent } from './components/task-view/task-view.component';
import { TaskCreateComponent } from './components/task-create/task-create.component';
import { SearchComponent } from './components/search/search.component';

export const routes: Routes = [
    { path: '', component: HelpComponent },
    { path: 'help', component: HelpComponent },
    { path: 'tasks', component: TaskListComponent },
    { path: 'login', component: LoginComponent },
    { path: 'users', component: UserListComponent },
    { path: 'task/create', component: TaskCreateComponent },
    { path: 'task/view/:id', component: TaskViewComponent },
    { path: 'task/edit/:id', component: TaskCreateComponent },
    { path: 'search', component: SearchComponent, },
];
