import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent
  },
  {
    path: 'routes',
    loadComponent: () => import('./pages/routes.component').then(m => m.RoutesComponent)
  },
  {
    path: 'routes/:id',
    loadComponent: () => import('./pages/route-detail.component').then(m => m.RouteDetailComponent)
  },
  {
    path: 'login',
    loadComponent: () => import('./pages/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./pages/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'profile',
    loadComponent: () => import('./pages/profile.component').then(m => m.ProfileComponent)
  }
];
