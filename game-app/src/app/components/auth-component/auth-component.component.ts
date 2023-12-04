import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';

@Component({
  selector: 'app-auth-component',
  templateUrl: './auth-component.component.html',
  styleUrls: ['./auth-component.component.css']
})
export class AuthComponentComponent {
  credentials = {
    username: '',
    password: ''
  };
  
  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.credentials).subscribe(
      success => {
        // Handle response here
        this.router.navigate(['/']); // Navigate to the home page or dashboard
      },
      error => {
        const register = confirm('User not found. Do you want to register?');
        if (register) {
          this.authService.register(this.credentials).subscribe(
            success => {
              // Handle successful registration
            },
            error => {
              // Handle registration error
            }
          );
        }
      }
    );
  }
  
}
