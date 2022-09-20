import { TestBed } from '@angular/core/testing';

import { NgbDateHandlerService } from './ngb-date-handler.service';

describe('DateHandlerService', () => {
  let service: NgbDateHandlerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NgbDateHandlerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
