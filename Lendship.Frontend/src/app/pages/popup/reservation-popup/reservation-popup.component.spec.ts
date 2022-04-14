import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReservationPopupComponent } from './reservation-popup.component';

describe('ReservationPopupComponent', () => {
  let component: ReservationPopupComponent;
  let fixture: ComponentFixture<ReservationPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReservationPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReservationPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
