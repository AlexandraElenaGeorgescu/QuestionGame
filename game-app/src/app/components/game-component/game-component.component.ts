import { Component, OnInit } from '@angular/core';
import { PlayGameResponse } from 'src/app/models/PlayGameResponse';
import { AuthService } from 'src/app/services/auth-service.service';
import { GameService } from 'src/app/services/game-service.service';
import { ChangeDetectorRef } from '@angular/core';

@Component({
  selector: 'app-game',
  templateUrl: './game-component.component.html',
  styleUrls: ['./game-component.component.css']
})
export class GameComponentComponent implements OnInit {
  currentQuestion: any;
  userAnswer: string = '';
  message: string = '';
  score: number = 0;
  username: string = ''; 

  constructor(private cdr: ChangeDetectorRef, private gameService: GameService, private authService: AuthService) { }

  ngOnInit(): void {
    const currentUser = this.authService.getCurrentUser();
    if (currentUser) {
      this.username = currentUser.username; 
      this.getNextQuestion();
      this.cdr.detectChanges();
    } else {
      this.message = "User not found. Please log in.";
    }
  }

  getNextQuestion(): void {
    this.gameService.getNextQuestion(this.username).subscribe(
      response => {
          if (response.value.hasOwnProperty('message')) {
              // It's a message, not a question. The user has won the game.
              this.message = "You win! Congrats!";
          } else {
              // It's a question. Continue the game as usual.
              this.currentQuestion = response.value;
              this.message = "All answers have lower letters and max 2 words! Good luck!";
          }
      },
      error => {
          console.error('Error getting next question', error);
          this.message = "Error fetching the next question!";
      }
    );
  }  

  submitAnswer(): void {
    if(!this.userAnswer) {
      this.message = "Please provide an answer.";
      return;
    }

    this.gameService.submitAnswer(this.username, this.userAnswer).subscribe(
      (data: any) => {  
          const gameResponse = data.value;
          this.score = gameResponse.game.score
          if(gameResponse.isCorrectAnswer){
              this.message = "Correct! Next question:";
              this.getNextQuestion();
          } else {
              this.message = "Incorrect! Game over.";
          }
      },
      error => {
          console.error('Error submitting answer', error);
          this.message = "Error submitting the answer!";
      }
  );
 }  
}
