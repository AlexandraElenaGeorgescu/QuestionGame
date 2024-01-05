import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Question } from '../models/Question';
import { Observable } from 'rxjs';
import { User } from '../models/User';

@Injectable({
  providedIn: 'root'
})
export class AdminServiceService {
  private baseUrl = 'https://localhost:7173'; // Update with your actual API URL

  constructor(private http: HttpClient) {}

  createQuestion(question: any) {
    return this.http.post(`${this.baseUrl}/api/Questions`, question);
  }

  getAllQuestions(): Observable<Question[]> {
    return this.http.get<Question[]>(`${this.baseUrl}/api/Questions`);
  }

  deleteQuestion(questionId: string) {
    return this.http.delete(`${this.baseUrl}/api/Questions/${questionId}`);
  }

  deleteUser(userId: string) {
    return this.http.delete(`${this.baseUrl}/User/${userId}`);
  }
  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseUrl}/User`); 
  }
}
