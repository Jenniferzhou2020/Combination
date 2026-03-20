import { HttpClient } from '@angular/common/http';
import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from './auth.service'
import { MatMenuModule } from '@angular/material/menu'
import { MatButtonModule } from '@angular/material/button';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, AsyncPipe } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthFacade } from './auth.facade';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
 // styleUrl: './app.component.css',
  standalone: true,
  imports: [RouterOutlet,MatButtonModule, MatMenuModule, FormsModule, NgFor, NgIf, AsyncPipe],
})
export class AppComponent implements OnInit {
  facade = inject(AuthFacade);
  constructor(private http: HttpClient, private auth: AuthService) { }

  ngOnInit() {
  }
  vm = {
    loading: this.facade.loading,
    error: this.facade.error,
    isAuth: this.facade.isAuthenticated
  };
  BackgroundStyle = 'background: linear-gradient(rgba(255, 255, 255, 0.8), rgba(255, 255, 255, 0.8)), min-height: 100vh;background-size: cover;';


  title = 'Angular application with Controller example';
}
