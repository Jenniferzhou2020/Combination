import { Component, inject } from '@angular/core';
import { AuthFacade } from '../auth.facade'
import { Observable } from 'rxjs';
import { WeatherForecast } from '../auth.modles';
import { HttpClient } from '@angular/common/http';
import { AuthService } from '../auth.service';
import { FormsModule } from '@angular/forms';
import { NgFor, NgIf, AsyncPipe } from '@angular/common';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  imports: [NgIf, NgFor, FormsModule, AsyncPipe],
  standalone:true
})
export class HomeComponent {

  facade = inject(AuthFacade);
  public forecasts$: Observable<WeatherForecast[]> = this.auth.getForecasts$()
  constructor(private http: HttpClient, private auth: AuthService) { }
}
