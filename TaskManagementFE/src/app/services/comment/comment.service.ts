import { Injectable } from '@angular/core';
import { ServerService } from '../server/server.service';
import { Comment } from '../../models/comment';
import { lastValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private server: ServerService) { }

  public async createComment(comment: Comment): Promise<Comment | undefined>
  {
    try
    {
      var observable = this.server.request('POST', '/api/Comment', comment);
      return await lastValueFrom(observable) as Promise<Comment>;
    }
    catch
    {
      return undefined;
    }
  }
  
  public async updateComment(comment: Comment, id: string): Promise<Comment | undefined>
  {
    try
    {
      var observable = this.server.request('PUT', `/api/Comment/${id}`, comment);
      return await lastValueFrom(observable) as Promise<Comment>;
    }
    catch
    {
      return undefined;
    }
  }

  public async deleteComment(id: string): Promise<Comment | undefined>
  {
    try
    {
      var observable = this.server.request('DELETE', `/api/Comment/${id}`);
      return await lastValueFrom(observable) as Promise<Comment>;
    }
    catch
    {
      return undefined;
    }
  }

  public async getComment(id: string): Promise<Comment | undefined>
  {
    try {
      var observable = this.server.request('GET', `/api/Comment/${id}`);
      return await lastValueFrom(observable) as Promise<Comment>;
    }
    catch (err) {
      return undefined;
    }
  }

  public async getComments() : Promise<Comment[] | undefined>
  {
    try
    {
      var observable = this.server.request('GET', '/api/Comment');
      return await lastValueFrom(observable) as Promise<Comment[]>;
    }
    catch
    {
      return undefined;
    }
  }
}
