import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) },
  { path: 'register', loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent) },
  { path: 'dashboard', loadComponent: () => import('./pages/dashboard/dashboard.component').then(m => m.DashboardComponent), canActivate: [authGuard] },
  { path: 'courses', loadComponent: () => import('./pages/courses/courses.component').then(m => m.CoursesComponent), canActivate: [authGuard] },
  { path: 'recommendations', loadComponent: () => import('./pages/recommendations/recommendations.component').then(m => m.RecommendationsComponent), canActivate: [authGuard] },
];