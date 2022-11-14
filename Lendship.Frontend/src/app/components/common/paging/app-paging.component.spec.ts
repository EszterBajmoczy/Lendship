import { ComponentFixture, TestBed } from '@angular/core/testing';
import {AppPagingComponent} from "./app-paging.component";


describe('AppSearchComponent', () => {
  let component: AppPagingComponent;
  let fixture: ComponentFixture<AppPagingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppPagingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppPagingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
