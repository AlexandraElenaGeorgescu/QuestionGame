import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DashboardAdminComponent } from './dashboard-admin.component';
import { AdminServiceService } from 'src/app/services/admin-service.service';
import { of, throwError } from 'rxjs';

describe('DashboardAdminComponent', () => {
  let component: DashboardAdminComponent;
  let fixture: ComponentFixture<DashboardAdminComponent>;
  let mockAdminService: jasmine.SpyObj<AdminServiceService>;

  beforeEach(async () => {
    mockAdminService = jasmine.createSpyObj('AdminServiceService', ['getAllQuestions', 'createQuestion', 'deleteQuestion', 'getAllUsers', 'deleteUser']);

    await TestBed.configureTestingModule({
      declarations: [ DashboardAdminComponent ],
      providers: [
        { provide: AdminServiceService, useValue: mockAdminService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load all questions and players on init', () => {
    const questions = [{ id: '1', text: 'Test?', correctAnswer: 'Yes' }];
    const users = [{ username: 'player1', role: 'Player', password: 'password' }];

    mockAdminService.getAllQuestions.and.returnValue(of(questions));
    mockAdminService.getAllUsers.and.returnValue(of(users));

    component.ngOnInit();

    expect(component.questions.length).toBe(1);
    expect(component.users.length).toBe(1);
  });

  it('should create a question and reload questions', () => {
    const newQuestion = { text: 'New Question', correctAnswer: 'Answer' };
    mockAdminService.createQuestion.and.returnValue(of(newQuestion));
    mockAdminService.getAllQuestions.and.returnValue(of([])); // Mock return empty array for simplicity

    component.newQuestion = newQuestion;
    component.createQuestion();

    expect(mockAdminService.createQuestion).toHaveBeenCalledWith(newQuestion);
    expect(mockAdminService.getAllQuestions).toHaveBeenCalled();
  });

  it('should delete a question and reload questions', () => {
    const questionId = '123';
    mockAdminService.deleteQuestion.and.returnValue(of({}));
    mockAdminService.getAllQuestions.and.returnValue(of([])); // Mock return empty array for simplicity

    component.deleteQuestion(questionId);

    expect(mockAdminService.deleteQuestion).toHaveBeenCalledWith(questionId);
    expect(mockAdminService.getAllQuestions).toHaveBeenCalled();
  });

  it('should delete a player and reload players', () => {
    const playerId = '123';
    mockAdminService.deleteUser.and.returnValue(of({}));
    mockAdminService.getAllUsers.and.returnValue(of([])); // Mock return empty array for simplicity

    component.deletePlayer(playerId);

    expect(mockAdminService.deleteUser).toHaveBeenCalledWith(playerId);
    expect(mockAdminService.getAllUsers).toHaveBeenCalled();
  });

  it('should handle error while fetching questions', () => {
    mockAdminService.getAllQuestions.and.returnValue(throwError(() => new Error('Error')));
    spyOn(console, 'error');

    component.loadAllQuestions();

    expect(console.error).toHaveBeenCalledWith('Error fetching questions', jasmine.any(Error));
  });
});
