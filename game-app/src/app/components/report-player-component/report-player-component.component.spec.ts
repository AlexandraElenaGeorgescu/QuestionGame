import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportPlayerComponentComponent } from './report-player-component.component';

describe('ReportPlayerComponentComponent', () => {
  let component: ReportPlayerComponentComponent;
  let fixture: ComponentFixture<ReportPlayerComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReportPlayerComponentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportPlayerComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
