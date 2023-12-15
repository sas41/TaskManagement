import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ServerService } from '../server/server.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(false);
  private token: string | undefined;
  private roles: string | undefined;
  private username: string | undefined;
  private id: string | undefined;

  get loggedInStatus() {
    return this.loggedIn.asObservable();
  }

  get isLoggedIn() {
    return this.loggedIn.value;
  }

  get isAdmin() {
    return this.roles?.includes('Admin');
  }

  get isManager() {
    return this.roles?.includes('Manager');
  }
  
  isOwner(id: string) {
    return this.id === id;
  }
  
  isOwnerOrAdmin(id: string) {
    return (this.id === id || this.roles?.includes('Admin'));
  }

  get getRoles() {
    return this.roles;
  }

  get getUsername() {
    return this.username;
  }

  get getId() {
    return this.id;
  }

  constructor(private router: Router, private server: ServerService) {
    const userData = localStorage.getItem('user');
    if (userData) {
      const user = JSON.parse(userData);
      this.token = user.token;
      this.server.setLoggedIn(true, this.token);
      this.loggedIn.next(true);
      this.roles = user.roles;
      this.username = user.username;
      this.id = user.id;
    }
  }

  login(credentials: any) {
    if (credentials.username !== '' && credentials.password !== '' )
    {
      return this.server.request('POST', '/api/User/Login', {
        username: credentials.username,
        password: credentials.password
      }).subscribe((response: any) => {
        if (response.success === true && response.token !== undefined && response.username !== undefined && response.roles !== undefined) {
          this.server.setLoggedIn(true, response.token);

          this.loggedIn.next(true);
          this.token = response.token;
          this.roles = response.roles;
          this.username = response.username;
          this.id = response.id;

          const userData = {
            token:  response.token,
            roles: response.roles,
            username: response.username,
            id: response.id
          };

          localStorage.setItem('user', JSON.stringify(userData));
          this.router.navigateByUrl('/tasks');
        }
      });
    }
    return null;
  }

  logout() {
    this.server.setLoggedIn(false);
    delete this.token;
    delete this.username;
    delete this.roles;
    delete this.id;

    this.loggedIn.next(false);
    localStorage.clear();
    this.router.navigate(['/']);
  }
}