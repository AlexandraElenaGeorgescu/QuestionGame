import { Component, OnInit } from '@angular/core';
import { Question } from 'src/app/models/Question';
import { User } from 'src/app/models/User';
import { AdminServiceService } from 'src/app/services/admin-service.service';

@Component({
  selector: 'app-dashboard-admin',
  templateUrl: './dashboard-admin.component.html',
  styleUrls: ['./dashboard-admin.component.css']
})
export class DashboardAdminComponent implements OnInit {
  newQuestion = { text: '', correctAnswer: '' };
  questions: Question[] = [];

  constructor(private adminService: AdminServiceService) {}

  ngOnInit() {
    this.loadAllQuestions();
    this.loadAllPlayers();
  }

  loadAllQuestions() {
    this.adminService.getAllQuestions().subscribe(
      (questions: Question[]) => {  // Use the Question model here
        this.questions = questions;
      },
      error => console.error('Error fetching questions', error)
    );
  }  

  createQuestion() {
    this.adminService.createQuestion(this.newQuestion).subscribe(
      response => {
        console.log('Question created', response);
        this.newQuestion = { text: '', correctAnswer: '' }; // Reset the form
        this.loadAllQuestions(); // Refresh the list of questions
      },
      error => console.error('Error creating question', error)
    );
  }

  deleteQuestion(questionId: string) {
    this.adminService.deleteQuestion(questionId).subscribe(
      response => {
        console.log('Question deleted', response);
        this.loadAllQuestions(); // Refresh the list of questions
      },
      error => console.error('Error deleting question', error)
    );
  }

  users: any[] = [];

  loadAllPlayers() {
    this.adminService.getAllUsers().subscribe(
      (users: any[]) => {
        this.users = users.filter(user => user.role === 'Player');
      },
      error => console.error('Error fetching users', error)
    );
  }

  deletePlayer(playerId: string) {
    this.adminService.deleteUser(playerId).subscribe(
      response => {
        console.log('Player deleted', response);
        this.loadAllPlayers(); // Refresh the list of players
      },
      error => console.error('Error deleting player', error)
    );
  }
}
