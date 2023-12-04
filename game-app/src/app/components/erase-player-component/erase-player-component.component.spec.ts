import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ErasePlayerComponentComponent } from './erase-player-component.component';

describe('ErasePlayerComponentComponent', () => {
  let component: ErasePlayerComponentComponent;
  let fixture: ComponentFixture<ErasePlayerComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ErasePlayerComponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ErasePlayerComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
