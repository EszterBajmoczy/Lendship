import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivatePopupComponent } from './private-popup.component';

describe('PrivatePopupComponent', () => {
  let component: PrivatePopupComponent;
  let fixture: ComponentFixture<PrivatePopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PrivatePopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivatePopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
