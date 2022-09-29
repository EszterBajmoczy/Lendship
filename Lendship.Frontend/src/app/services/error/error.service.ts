import { Injectable } from '@angular/core';
import {HttpErrorResponse} from "@angular/common/http";
import {throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ErrorService {
  error: HttpErrorResponse | null = null;

  constructor() { }

  handleError(error: HttpErrorResponse | undefined) {
    if (error !== undefined && error.status === 0) {
      this.error = error;
      console.error('An error occurred:', error.error);
    }

    return throwError(() => new Error('Something bad happened; please try again later.'));
  }

  getError(): HttpErrorResponse | null {
    return this.error;
  }

  setError(error: HttpErrorResponse) {
    this.error = error;
  }

}
