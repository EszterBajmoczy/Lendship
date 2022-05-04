import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {AuthService} from "../auth/auth.service";
import {map, Observable, throwError} from "rxjs";
import {Reservation} from "../../models/reservation";
import {IAvailability} from "../../models/availability";
import {environment} from "../../../environments/environment";
import {IReservationDetail} from "../../models/reservation-detail";
import {Advertisement} from "../../models/advertisement";

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

  getReservationForAdvertisement(advertisementId: number): Observable<IAvailability[]> {
    return this.http.get<IAvailability[]>(this.baseUrl + advertisementId, { headers: this.headers})
      .pipe(
        map((response: IAvailability[]) => this.convertAvailabilityDateFormats(response)),
        catchError(this.handleError));
  }

  reserve(advertisementId: number, res: Reservation){
    return this.http.post(this.baseUrl + "?advertisementId=" + advertisementId, res, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getReservationsForUsersAdvertisement(): Observable<IReservationDetail[]>  {
    return this.http.get<IReservationDetail[]>(this.baseUrl + "for", { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => this.convertReservationDateFormats(response)),
        catchError(this.handleError));
  }

  getUsersReservations(): Observable<IReservationDetail[]> {
    return this.http.get<IReservationDetail[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => this.convertReservationDateFormats(response)),
        catchError(this.handleError));
  }

  updateReservationsState(resId: number, state: string) {
    let queryString = "?reservationId=" + resId + "&state=" + state;

    return this.http.post(this.baseUrl + "state" + queryString, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  admitReservation(resId: number){
    return this.http.post(this.baseUrl + "admit/" + resId, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  private convertReservationDateFormats(res: IReservationDetail[]){
    res.forEach(r => {
      r.dateTo = new Date(r.dateTo ?? '');
      r.dateFrom = new Date(r.dateFrom ?? '');
    })
    return res;
  }

  private convertAvailabilityDateFormats(ads: IAvailability[]){
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
