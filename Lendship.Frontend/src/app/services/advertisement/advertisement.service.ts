import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { Advertisement } from "../../models/advertisement";
import { AuthService } from "../auth/auth.service";

@Injectable({
  providedIn: 'root'
})
export class AdvertisementService {

  constructor(private http: HttpClient, private authService: AuthService) { }

  getAdvertisements(): Observable<Advertisement[]>{
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.getAccessToken()}`
    })

    return this.http.get<Advertisement[]>("https://localhost:44377/Advertisement", { headers: headers})
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
