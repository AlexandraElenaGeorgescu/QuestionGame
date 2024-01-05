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
  correctAnswer: string = '';

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

  resetGameState(): void {
    // Reset game state logic here
    this.score = 0;
    this.currentQuestion = null;
    // Call any other necessary services to reset the backend state if needed
  }

  // Called when the user clicks the 'Play' button
  startGame(): void {
    this.gameService.startGame(this.username).subscribe(
      (game: any) => {
        this.message = "New game started! Good luck!";
        this.score = game.score;
        this.userAnswer = '';  // Reset the answer
        this.getNextQuestion();  // Fetch the first question for the new game
      },
      error => {
        console.error('Error starting new game', error);
        this.message = "Error starting a new game!";
      }
    );
  }  

// Called when the user submits an answer
submitAnswer(): void {
  if (!this.userAnswer) {
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
        this.correctAnswer = gameResponse.CorrectAnswer;;
        console.log(gameResponse);
        this.message = `Incorrect! Game over. The correct answer was ${this.correctAnswer}.`;
      }
    },
    error => {
      console.error('Error submitting answer', error);
      this.message = "Error submitting the answer!";
    }
  );
}

}
