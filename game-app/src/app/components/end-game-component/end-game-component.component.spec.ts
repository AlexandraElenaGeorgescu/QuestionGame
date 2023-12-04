import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EndGameComponentComponent } from './end-game-component.component';

describe('EndGameComponentComponent', () => {
  let component: EndGameComponentComponent;
  let fixture: ComponentFixture<EndGameComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EndGameComponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EndGameComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
