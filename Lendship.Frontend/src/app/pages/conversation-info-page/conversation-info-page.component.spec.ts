import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConversationInfoPageComponent } from './conversation-info-page.component';

describe('ConversationInfoPageComponent', () => {
  let component: ConversationInfoPageComponent;
  let fixture: ComponentFixture<ConversationInfoPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ConversationInfoPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ConversationInfoPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
