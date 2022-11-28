import { ComponentFixture, TestBed } from '@angular/core/testing';

import {EvaluationPopupComponent} from './evaluation-popup.component';

describe('EvaluationPopupComponent', () => {
  let component: EvaluationPopupComponent;
  let fixture: ComponentFixture<EvaluationPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EvaluationPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EvaluationPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
