import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

// auth.service.ts
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:7173/Auth';
  
  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
    })
  }

  constructor(private http: HttpClient) {}

  login(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, JSON.stringify(user), this.httpOptions);
  }  

  register(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`,  JSON.stringify(user), this.httpOptions);
  }  
}