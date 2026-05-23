import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Progress {
  id: number;
  authId: number;
  courseId: number;
  totalLessons: number;
  completedLessons: number;
  progressPercentage: number;
  lastUpdated: string;
}

@Injectable({
  providedIn: 'root'
})
export class ProgressService {
 private apiUrl = 'http://localhost:5005/progress';

  constructor(private http: HttpClient) {}

  enroll(courseId: number, totalLessons: number): Observable<Progress> {
    return this.http.post<Progress>(`${this.apiUrl}/enroll`, { courseId, totalLessons });
  }

  completeLesson(courseId: number): Observable<Progress> {
    return this.http.post<Progress>(`${this.apiUrl}/complete-lesson`, { courseId });
  }

  getMyProgress(): Observable<Progress[]> {
    return this.http.get<Progress[]>(`${this.apiUrl}/my-progress`);
  }
}