import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementsPageComponent } from './advertisements-page.component';

describe('AdvertisementsPageComponent', () => {
  let component: AdvertisementsPageComponent;
  let fixture: ComponentFixture<AdvertisementsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertisementsPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertisementsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
