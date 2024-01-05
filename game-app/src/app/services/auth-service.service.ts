import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, tap } from "rxjs";
import { User } from "../models/User";

// auth.service.ts
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = 'https://localhost:7173/Auth';
  private currentUser: User | null = null;

  readonly httpOptions = {
    headers: new HttpHeaders({
      'Content-Type':  'application/json',
    })
  }

  constructor(private http: HttpClient) {
  }

  login(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/login`, JSON.stringify(user), this.httpOptions).pipe(
      tap((res: any) => {
        this.setCurrentUser(res.userData); // Assuming res.user contains user data
      })
    );
  }
  
  register(user: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/register`,  JSON.stringify(user), this.httpOptions);
  }  
  logout() {
    // Clear the current user and remove from local storage
    this.currentUser = null;
    localStorage.removeItem('currentUser');
  }

  getCurrentUser(): User | null {
    return this.currentUser;
  }

  private setCurrentUser(user: User) {
    this.currentUser = user;
    localStorage.setItem('currentUser', JSON.stringify(user));
  }

  private loadUser() {
    // Load user from local storage if available
    const userData = localStorage.getItem('currentUser');
    if (userData) {
      this.currentUser = JSON.parse(userData);
    }
  }
}