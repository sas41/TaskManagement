import { Component } from '@angular/core';
import { NgFor, NgIf, formatDate } from '@angular/common';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import {ActivatedRoute, Router} from "@angular/router";
import { TaskService } from '../../services/task/task.service';
import { Task } from '../../models/task';

const defaultDueInDays = 7;
const defaultDueIn = 1000*60*60*24*defaultDueInDays;

@Component({
  selector: 'app-task-create',
  standalone: true,
  imports: [ReactiveFormsModule, NgFor, NgIf],
  templateUrl: './task-create.component.html',
  styleUrl: './task-create.component.css'
})
export class TaskCreateComponent {
  public taskTypes = Task.TaskTypes;
  public taskStatuses = Task.TaskStatuses;
  public model:Task = new Task('', '', formatDate(new Date(new Date().getTime() + defaultDueIn), 'yyyy-MM-dd', 'en'), 'New', 'Task');

  public isUpdate = false;
  public error = false;
  private submitted = false;

  taskForm = this.formBuilder.group(
    this.model
  );

  constructor(private router: Router, private route: ActivatedRoute, private formBuilder: FormBuilder, private taskService: TaskService)
  {
    this.route.params.subscribe(params => {
      if (params['id'])
      {
        this.taskService.getTask(params['id']).then((task) => {
          if (task) {
            this.setModel(task);
          }
          else
          {
            this.router.navigate(['/task/create']);
          }
        });
      }
    });
  }

  public setModel(model: Task) {
    this.isUpdate = true;
    this.model = model;
    this.model.requiredBy = formatDate(model.requiredBy, 'yyyy-MM-dd', 'en');
    this.taskForm.patchValue(model);
  }

  async onSubmit() {
    this.error = false;
    this.submitted = false;

    if (this.taskForm.valid)
    {
      try
      {
        var result;
        var data = this.taskForm.value as Task;
        if (this.isUpdate && data.id)
        {
          result = await this.taskService.updateTask(data, data.id);
        } 
        else
        {
          result = await this.taskService.createTask(data);
        }
        if (result?.id)
        {
          this.router.navigate([`/task/view/${result.id}`]);
        }
      }
      catch (err)
      {
        console.log(err);
        this.error = true;
      }
    }
    else
    {
      this.submitted = true;
    }
  }
}
