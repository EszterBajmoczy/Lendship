import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementInfoPageComponent } from './advertisement-info-page.component';

describe('AdvertisementInfoPageComponent', () => {
  let component: AdvertisementInfoPageComponent;
  let fixture: ComponentFixture<AdvertisementInfoPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdvertisementInfoPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvertisementInfoPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
