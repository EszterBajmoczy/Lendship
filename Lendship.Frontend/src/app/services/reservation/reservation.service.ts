import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {AuthService} from "../auth/auth.service";
import {Advertisement} from "../../models/advertisement";
import {map, throwError} from "rxjs";
import {IReservation, Reservation} from "../../models/reservation";
import {IAvailability} from "../../models/availability";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.getAccessToken()}`
    });
  }

  getReservationForAdvertisement(advertisementId: number){
    return this.http.get<IAvailability[]>("https://localhost:44377/Reservation/" + advertisementId, { headers: this.headers})
      .pipe(
        map((response: IAvailability[]) => this.convertDateFormat(response)),
        catchError(this.handleError));
  }

  reserve(advertisementId: number, res: Reservation){
    console.log(res);
    return this.http.post("https://localhost:44377/Reservation/?advertisementId=" + advertisementId, res, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  convertDateFormat(ads: IAvailability[]){
    ads.forEach(av => {
      av.dateTo = new Date(av.dateTo ?? '');
      av.dateFrom = new Date(av.dateFrom ?? '');
    })
    return ads;
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
