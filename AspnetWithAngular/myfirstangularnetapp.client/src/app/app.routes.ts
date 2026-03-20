import { Routes } from '@angular/router';
//import { LoginComponent } from './login/login.component';
import { authGuard } from './auth.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
  //{ path: '', component: HomeComponent },
  { path: '', loadComponent: () => import('./home/home.component').then(m => m.HomeComponent), canActivate: [authGuard] },
  // { path: '**', redirectTo: '' }
];
