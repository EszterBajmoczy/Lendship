import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {AuthService} from "../auth/auth.service";
import {catchError} from "rxjs/operators";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private baseUrl: string;
  private headers: HttpHeaders;

  constructor(private http:HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Image/";
    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getAccessToken()}`,
    });
  }

  upload(advertisementId: number, files: File[]):Observable<any> {
    const formData = new FormData();
    files.forEach((file) => {
      formData.append(file.name, file);
    })
    console.log(this.baseUrl + advertisementId);
    return this.http.post(this.baseUrl + advertisementId, formData, {headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  delete(advertisementId: number, fileNames: string[]){
    fileNames.forEach(fileName => {
      console.log("!!!!!");
      console.log(this.baseUrl + advertisementId + "/" + this.simplifyImageName(fileName));
      return this.http.delete(this.baseUrl + advertisementId + "/" + this.simplifyImageName(fileName), {headers: this.headers})
        .subscribe(s => {console.log("sub");})
    })
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

  private simplifyImageName(name: string): string {
    let i = name.lastIndexOf("\\");
    console.log(name.substring(i+1));
    return name.substring(i + 1);
  }
}
