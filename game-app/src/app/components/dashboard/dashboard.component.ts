import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth-service.service';
import { GameService } from 'src/app/services/game-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  score?: number;
  username?: string;

  constructor(
    private gameService: GameService, 
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadPlayerInfo();
  }

  loadPlayerInfo() {
    this.username = this.authService.getCurrentUser()?.username; 
    this.gameService.getPlayerScore(this.username).subscribe(
      (data) => {
        this.score = data.score; 
      },
      (error) => {
        console.error('Error fetching score', error);
      }
    );
  }

  playGame() {
    this.router.navigate(['/play']);
  }

  logout() {
    this.authService.logout(); 
    this.router.navigate(['/login']); 
  }
}
