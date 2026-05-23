import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Recommendation {
  courseId: number;
  title: string;
  category: string;
  description: string;
  price: number;
}

@Injectable({
  providedIn: 'root'
})
export class RecommendationService {
  private apiUrl = 'http://localhost:5006/recommendations';

  constructor(private http: HttpClient) {}

  getRecommendations(): Observable<Recommendation[]> {
    return this.http.get<Recommendation[]>(this.apiUrl);
  }
}