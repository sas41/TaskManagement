import { Injectable } from '@angular/core';
import { ServerService } from '../server/server.service';
import { Task } from '../../models/task';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private server: ServerService) { }

  public async createTask(task: Task): Promise<Task | undefined>
  {
    try
    {
      var observable = this.server.request('POST', '/api/Task', task);
      return await lastValueFrom(observable) as Promise<Task>;
    }
    catch
    {
      return undefined;
    }
  }
  
  public async updateTask(task: Task, id: string): Promise<Task | undefined>
  {
    try
    {
      var observable = this.server.request('PUT', `/api/Task/${id}`, task);
      return await lastValueFrom(observable) as Promise<Task>;
    }
    catch
    {
      return undefined;
    }
  }

  public async deleteTask(id: string): Promise<Task | undefined>
  {
    try
    {
      var observable = this.server.request('DELETE', `/api/Task/${id}`);
      return await lastValueFrom(observable) as Promise<Task>;
    }
    catch
    {
      return undefined;
    }
  }

  public async getTask(id: string): Promise<Task | undefined>
  {
    try {
      var observable = this.server.request('GET', `/api/Task/${id}`);
      return await lastValueFrom(observable) as Promise<Task>;
    }
    catch (err) {
      return undefined;
    }
  }

  public async getTasks(search? :string) : Promise<Task[] | undefined>
  {
    try
    {
      var observable;
      if (search)
      {
        observable = this.server.request('GET', `/api/Task?search=${search}`);
      }
      else
      {
        observable = this.server.request('GET', '/api/Task');
      }
      return await lastValueFrom(observable) as Promise<Task[]>;
    }
    catch
    {
      return undefined;
    }
  }

  public async assignToTask(taskId: string, users: string[]) :  Promise<Task | undefined>
  {
    try
    {
      var observable = this.server.request('POST', `/api/Task/Assign/${taskId}`, users);
      return await lastValueFrom(observable) as Promise<Task>;
    }
    catch
    {
      return undefined;
    }
  }

  // Does this really belong here?
  public async getPossibleAssignees() : Promise<any[] | undefined>
  {
    try
    {
      var observable = this.server.request('GET', '/api/User');
      return await lastValueFrom(observable) as Promise<any[]>;
    }
    catch
    {
      return undefined;
    }
  }

}
