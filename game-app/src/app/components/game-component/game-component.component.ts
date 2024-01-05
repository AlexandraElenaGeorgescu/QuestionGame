// game.component.ts
import { Component, OnInit } from '@angular/core';
import { GameService } from 'src/app/services/game-service.service';

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

  constructor(private gameService: GameService) { }

  ngOnInit(): void {
    this.getNextQuestion();
  }

  getNextQuestion(): void {
    // Fetch the next question from the backend
    this.gameService.getNextQuestion().subscribe(
      data => this.currentQuestion = data,
      error => console.error('Error getting next question', error)
    );
  }

  submitAnswer(): void {
    this.gameService.submitAnswer(this.currentQuestion.id, this.userAnswer).subscribe(
      data => {
        this.score = data.score;
        if(data.correct){
          this.message = "Correct! Next question:";
          this.getNextQuestion();
        } else {
          this.message = "Incorrect! Game over.";
          // Optionally navigate back to dashboard or show final score
        }
      },
      error => console.error('Error submitting answer', error)
    );
  }
}
