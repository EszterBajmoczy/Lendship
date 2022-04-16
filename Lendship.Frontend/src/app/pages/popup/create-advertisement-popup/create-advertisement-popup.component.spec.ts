import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAdvertisementPopupComponent } from './create-advertisement-popup.component';

describe('ReservationPopupComponent', () => {
  let component: CreateAdvertisementPopupComponent;
  let fixture: ComponentFixture<CreateAdvertisementPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateAdvertisementPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateAdvertisementPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
