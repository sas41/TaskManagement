import { Component } from '@angular/core';
import { NgIf } from '@angular/common';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [NgIf],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  constructor(private router: Router, public auth: AuthService) {}
  async onSearchSubmit()
  {
    var query = (<HTMLInputElement>document.getElementById('search')).value;
    if(query != '')
    {
      this.router.navigate([`search`], { queryParams: { query: query }});
    }
  }
}
