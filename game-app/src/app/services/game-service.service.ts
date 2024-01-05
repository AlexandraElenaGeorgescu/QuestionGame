import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  private baseUrl = 'https://localhost:7173/api/Game'; 

  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
    })
  };

  constructor(private http: HttpClient) {}

  getPlayerScore(username?: string): Observable<any> {
    return this.http.get(`${this.baseUrl}/score/${username}`, this.httpOptions);
  }

  submitScore(username?: string, score?: number): Observable<any> {
    const payload = { username, score };
    return this.http.post(`${this.baseUrl}/submit-score`, JSON.stringify(payload), this.httpOptions);
  }

  submitAnswer(questionId: string, userAnswer: string): Observable<any> {
    const payload = { questionId, answer: userAnswer };
    return this.http.post(`${this.baseUrl}/submit-answer`, JSON.stringify(payload), this.httpOptions);
  }

  getNextQuestion(): Observable<any> {
    return this.http.get(`${this.baseUrl}/next-question`, this.httpOptions);
  }

  playGame(username?: string): Observable<any> {
    const payload = { username };
    return this.http.post(`${this.baseUrl}/play`, JSON.stringify(payload), this.httpOptions);
  }
}
