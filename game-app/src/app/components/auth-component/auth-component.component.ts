// auth-component.component.ts
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { User } from 'src/app/models/User';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-auth-component',
  templateUrl: './auth-component.component.html',
  styleUrls: ['./auth-component.component.css']
})
export class AuthComponentComponent {
  user: User = {
    username: "",
    password: "",
    role: ""
  };
  isModerator = false; 
  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.user.role = this.isModerator ? 'Moderator' : 'Player';
    this.authService.login(this.user).subscribe(
      success => {
        // Navigate based on role
        if (success.role === 'Moderator') {
          this.router.navigate(['/dashboard-admin']);
        } else {
          this.router.navigate(['/dashboard']);
        }
      },
      error => {
        // If unauthorized, ask if they want to register
        const register = confirm('User not found. Do you want to register?');
        if (register) {
          this.authService.register(this.user).subscribe(
            success => {
              console.log('Registration successful', success);
              this.router.navigate(['/dashboard']);
            },
            error => {
              console.error('Registration failed', error);
            }
          );
        }
      }
    );
  }
}
