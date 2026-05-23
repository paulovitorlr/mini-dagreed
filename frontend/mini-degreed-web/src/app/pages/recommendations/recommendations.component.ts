import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { RecommendationService, Recommendation } from '../../core/services/recommendation.service';
import { ProgressService } from '../../core/services/progress.service';

@Component({
  selector: 'app-recommendations',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './recommendations.component.html',
  styleUrl: './recommendations.component.scss'
})
export class RecommendationsComponent implements OnInit {
  recommendations: Recommendation[] = [];
  loading = true;

  constructor(
    private recommendationService: RecommendationService,
    private progressService: ProgressService
  ) {}

  ngOnInit() {
    this.recommendationService.getRecommendations().subscribe({
      next: (data) => {
        this.recommendations = data;
        this.loading = false;
      },
      error: () => {
        console.error('Erro ao carregar recomendações');
        this.loading = false;
      }
    });
  }

  enroll(courseId: number) {
    this.progressService.enroll(courseId, 10).subscribe({
      next: () => {
        this.recommendations = this.recommendations.filter(r => r.courseId !== courseId);
      }
    });
  }
}