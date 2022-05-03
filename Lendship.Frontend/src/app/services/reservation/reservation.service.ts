import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {AuthService} from "../auth/auth.service";
import {map, throwError} from "rxjs";
import {Reservation} from "../../models/reservation";
import {IAvailability} from "../../models/availability";
import {environment} from "../../../environments/environment";
import {IReservationDetail} from "../../models/reservation-detail";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private baseUrl: string;
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Reservation/";
    this.headers = authService.getHeaders();
  }

  getReservationForAdvertisement(advertisementId: number){
    return this.http.get<IAvailability[]>(this.baseUrl + advertisementId, { headers: this.headers})
      .pipe(
        map((response: IAvailability[]) => this.convertDateFormats(response)),
        catchError(this.handleError));
  }

  reserve(advertisementId: number, res: Reservation){
    return this.http.post(this.baseUrl + "?advertisementId=" + advertisementId, res, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getReservationsForUsersAdvertisement()  {
    return this.http.get<IReservationDetail[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => {
          response.forEach(r => {
            r.dateTo = new Date(r.dateTo ?? '');
            r.dateFrom = new Date(r.dateFrom ?? '');
          })
        }),
        catchError(this.handleError));
  }

  getUsersReservations() {
    return this.http.get<IReservationDetail[]>(this.baseUrl + "/for", { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => {
          response.forEach(r => {
            r.dateTo = new Date(r.dateTo ?? '');
            r.dateFrom = new Date(r.dateFrom ?? '');
          })
        }),
        catchError(this.handleError));
  }

  private convertDateFormats(ads: IAvailability[]){
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
