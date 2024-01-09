import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AuthComponentComponent } from './auth-component.component';
import { AuthService } from 'src/app/services/auth-service.service';
import { Router } from '@angular/router';
import { of, throwError } from 'rxjs';

describe('AuthComponentComponent', () => {
  let component: AuthComponentComponent;
  let fixture: ComponentFixture<AuthComponentComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    // Create mock AuthService
    mockAuthService = jasmine.createSpyObj('AuthService', ['login', 'register']);

    // Create mock Router
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [ AuthComponentComponent ],
      providers: [
        { provide: AuthService, useValue: mockAuthService },
        { provide: Router, useValue: mockRouter }
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should login and navigate to dashboard-admin for moderators', () => {
    const user = { username: 'moderator', password: 'password', role: 'Moderator' };
    mockAuthService.login.and.returnValue(of(user));
    component.user = user;
    component.onSubmit();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/dashboard-admin']);
  });

  it('should login and navigate to dashboard for players', () => {
    const user = { username: 'player', password: 'password', role: 'Player' };
    mockAuthService.login.and.returnValue(of(user));
    component.user = user;
    component.onSubmit();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/dashboard']);
  });

  it('should prompt for registration on failed login', () => {
    spyOn(window, 'confirm').and.returnValue(true);
    mockAuthService.login.and.returnValue(throwError(() => new Error('Unauthorized')));
    mockAuthService.register.and.returnValue(of({}));

    component.user = { username: 'newuser', password: 'password', role: 'Player' };
    component.onSubmit();

    expect(mockAuthService.register).toHaveBeenCalled();
  });

});
