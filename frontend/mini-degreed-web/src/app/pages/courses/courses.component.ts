import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CourseService, Course } from '../../core/services/course.service';
import { ProgressService } from '../../core/services/progress.service';

@Component({
  selector: 'app-courses',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './courses.component.html',
  styleUrl: './courses.component.scss'
})
export class CoursesComponent implements OnInit {
  courses: Course[] = [];
  enrolledCourseIds: number[] = [];
  message = '';

  constructor(
    private courseService: CourseService,
    private progressService: ProgressService
  ) {}

  ngOnInit() {
    this.courseService.getAll().subscribe({
      next: (data) => this.courses = data,
      error: () => console.error('Erro ao carregar cursos')
    });

    this.progressService.getMyProgress().subscribe({
      next: (data) => this.enrolledCourseIds = data.map(p => p.courseId),
      error: () => console.error('Erro ao carregar progresso')
    });
  }

  isEnrolled(courseId: number): boolean {
    return this.enrolledCourseIds.includes(courseId);
  }

  enroll(courseId: number) {
    this.progressService.enroll(courseId, 10).subscribe({
      next: () => {
        this.enrolledCourseIds.push(courseId);
        this.message = 'Matriculado com sucesso!';
        setTimeout(() => this.message = '', 2000);
      },
      error: () => this.message = 'Erro ao matricular.'
    });
  }
}