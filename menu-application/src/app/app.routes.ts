import { Routes } from '@angular/router';

export const routes: Routes = [
    {
    path: '',
    pathMatch: 'full',
    loadComponent: () => import('./home/home.component').then(m => m.HomeComponent),
    title: 'Home'
  },
  {
    path: 'login',
    loadComponent: () => import('./components/login/login.component').then(m => m.LoginComponent),
    title: 'Login'
  },
  {
    path: 'products',
    loadChildren: () => import('./products/products.routes').then(m => m.PRODUCT_ROUTES)
  },
  {
    path: 'account',
    loadChildren: () => import('./account/account.routes').then(m => m.ACCOUNT_ROUTES)
  },
  {
    path: '**',
    loadComponent: () =>
      import('./home/home.component').then(m => m.HomeComponent), // simple 404 fallback
    title: 'Not Found'
  }
];
