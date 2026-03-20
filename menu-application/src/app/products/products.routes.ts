// src/app/products/products.routes.ts
import { Routes } from '@angular/router';
import { ProductsShellComponent } from './products-shell/products-shell.component';


export const PRODUCT_ROUTES: Routes = [
  {
    path: '',
    component: ProductsShellComponent,
    children: [
      {
        path: '',
        pathMatch: 'full',
        redirectTo: 'list'
      },
      {
        path: 'list',
        loadComponent: () => import('./products-list/products-list.component').then(m => m.ProductsListComponent),
        title: 'Products'
      },
      {
        path: ':id',
        loadComponent: () => import('./products-detail/products-detail.component').then(m => m.ProductsDetailComponent),
        title: 'Product Detail'
      }
    ]
  }
];