import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {AuthService} from "../auth/auth.service";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  headers: HttpHeaders;

  constructor(private http:HttpClient, private authService: AuthService) {
    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getAccessToken()}`,
    });
  }

  upload(advertisementId: number, files: File[]):Observable<any> {
    const formData = new FormData();
    files.forEach((file) => {
      formData.append(file.name, file);
    })
    return this.http.post("https://localhost:44377/Image/" + advertisementId, formData, {headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    console.log("Error auth");
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}