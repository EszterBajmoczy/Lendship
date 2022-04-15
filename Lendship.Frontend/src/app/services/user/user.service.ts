import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { map, Observable, throwError } from 'rxjs';
import { AuthService } from "../auth/auth.service";
import { UserDetail } from "../../models/user-detail";
import { GeocodingService} from "../geocoding/geocoding.service";
import {AdvertisementDetail} from "../../models/advertisement-detail";
import {EvaluationAdvertiser, EvaluationLender} from "../../models/evaluations";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  headers: HttpHeaders

  constructor(private http: HttpClient, private authService: AuthService, private geoCodingService: GeocodingService) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.getAccessToken()}`
    });
  }

  getUser(): Observable<UserDetail>{
    return this.http.get<UserDetail>("https://localhost:44377/Profile", { headers: this.headers})
      .pipe(
        map((response: UserDetail) => this.setLocation(response)),
        catchError(this.handleError));
  }

  getOtherUser(id: string): Observable<UserDetail>{
    return this.http.get<UserDetail>("https://localhost:44377/Profile/" + id, { headers: this.headers})
      .pipe(
        map((response: UserDetail) => this.setLocation(response)),
        catchError(this.handleError));
  }

  getEvaluationAdvertiserUser(id: string): Observable<EvaluationAdvertiser[]>{
    return this.http.get<EvaluationAdvertiser[]>("https://localhost:44377/EvaluationAdvertiser/" + id, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getEvaluationLenderUser(id: string): Observable<EvaluationLender[]>{
    return this.http.get<EvaluationLender[]>("https://localhost:44377/EvaluationLender/" + id, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  setLocation(user: UserDetail): UserDetail{
    this.geoCodingService.getAddress(user.latitude, user.longitude)
      .subscribe(location => {
        user.location = location["city"]
      });
    return user;
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
