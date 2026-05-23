import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/services/auth.service';
import { ProgressService, Progress } from '../../core/services/progress.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent implements OnInit {
  progresses: Progress[] = [];

  constructor(
    private progressService: ProgressService,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit() {
    this.progressService.getMyProgress().subscribe({
      next: (data) => this.progresses = data,
      error: () => console.error('Erro ao carregar progresso')
    });
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }
}