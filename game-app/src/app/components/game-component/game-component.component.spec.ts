import { ComponentFixture, TestBed } from '@angular/core/testing';
import { GameComponentComponent } from './game-component.component';
import { AuthService } from 'src/app/services/auth-service.service';
import { GameService } from 'src/app/services/game-service.service';
import { of, throwError } from 'rxjs';

describe('GameComponentComponent', () => {
  let component: GameComponentComponent;
  let fixture: ComponentFixture<GameComponentComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockGameService: jasmine.SpyObj<GameService>;

  beforeEach(async () => {
    mockAuthService = jasmine.createSpyObj('AuthService', ['getCurrentUser']);
    mockGameService = jasmine.createSpyObj('GameService', ['startGame', 'submitAnswer', 'getNextQuestion']);

    await TestBed.configureTestingModule({
      declarations: [ GameComponentComponent ],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: GameService, useValue: mockGameService }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GameComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should start a game', () => {
    const mockUser = { username: 'testuser', password: 'pass', role: 'Player' };
    mockAuthService.getCurrentUser.and.returnValue(mockUser);
    const mockResponse = { score: 0 };
    mockGameService.startGame.and.returnValue(of(mockResponse));

    component.startGame();

    expect(component.message).toBe("New game started! Good luck!");
    expect(component.score).toBe(0);
  });

  it('should handle correct answer submission', () => {
    const mockResponse = { game: { score: 1 }, isCorrectAnswer: true };
    mockGameService.submitAnswer.and.returnValue(of({ value: mockResponse }));

    component.submitAnswer();

    expect(component.message).toContain("Correct!");
    expect(component.score).toBe(1);
  });

  it('should handle incorrect answer submission', () => {
    const mockResponse = { game: { score: 1 }, isCorrectAnswer: false };
    mockGameService.submitAnswer.and.returnValue(of({ value: mockResponse }));

    component.submitAnswer();

    expect(component.message).toContain("Incorrect!");
  });

  it('should handle error when starting a new game', () => {
    mockGameService.startGame.and.returnValue(throwError(() => new Error('Error')));
    spyOn(console, 'error');

    component.startGame();

    expect(console.error).toHaveBeenCalledWith('Error starting new game', jasmine.any(Error));
    expect(component.message).toBe("Error starting a new game!");
  });

});
