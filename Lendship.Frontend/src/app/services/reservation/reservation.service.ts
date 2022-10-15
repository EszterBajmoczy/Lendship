import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import {AuthService} from "../auth/auth.service";
import {map, Observable } from "rxjs";
import {Reservation} from "../../models/reservation";
import {IAvailability} from "../../models/availability";
import {environment} from "../../../environments/environment";
import {IReservationDetail} from "../../models/reservation-detail";
import {IReservationBasic} from "../../models/reservation-basic";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private readonly baseUrl: string;
  private readonly headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Reservation/";
    this.headers = authService.getHeaders();
  }

  getReservationForAdvertisement(advertisementId: number): Observable<IAvailability[]> {
    return this.http.get<IAvailability[]>(this.baseUrl + advertisementId, { headers: this.headers})
      .pipe(
        map((response: IAvailability[]) => this.convertAvailabilityDateFormats(response)));
  }

  reserve(advertisementId: number, res: Reservation){
    return this.http.post(this.baseUrl + "?advertisementId=" + advertisementId, res, { headers: this.headers});
  }

  getReservationsForUsersAdvertisement(): Observable<IReservationDetail[]>  {
    return this.http.get<IReservationDetail[]>(this.baseUrl + "for", { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => this.convertReservationDateFormats(response)));
  }

  getUsersReservations(): Observable<IReservationDetail[]> {
    return this.http.get<IReservationDetail[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((response: IReservationDetail[]) => this.convertReservationDateFormats(response)));
  }

  updateReservationsState(resId: number, state: string) {
    let queryString = "?reservationId=" + resId + "&state=" + state;

    return this.http.post(this.baseUrl + "state/" + queryString, { headers: this.headers});
  }

  admitReservation(resId: number){
    return this.http.post(this.baseUrl + "admit/" + resId, { headers: this.headers});
  }

  getReservationBasics(): Observable<IReservationBasic[]> {
    console.log(this.baseUrl + "recent")
    return this.http.get<IReservationBasic[]>(this.baseUrl + "recent", { headers: this.headers});
  }

  getReservationToken(resId: number, closing: boolean): Observable<string> {
    return this.http.get<string>(this.baseUrl + "reservationtoken/" + resId + "/" + closing, { headers: this.headers});
  }

  validateReservationToken(token: string): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + "reservationtoken/" + token, { headers: this.headers});
  }

  private convertReservationDateFormats(res: IReservationDetail[]){
    res.forEach(r => {
      r.dateTo = new Date(r.dateTo ?? '');
      r.dateFrom = new Date(r.dateFrom ?? '');
      r.dateToString = this.dateToString(r.dateTo);
      r.dateFromString = this.dateToString(r.dateFrom);

    })
    return res;
  }

  private dateToString(date: Date): string {
    if (date.getUTCMonth() + 1 < 10 && date.getUTCDate() + 1 < 10) {
      return `${date.getUTCFullYear()}-0${date.getUTCMonth() + 1}-0${date.getUTCDate() + 1}`;
    } else if (date.getUTCMonth() + 1 < 10) {
      return `${date?.getUTCFullYear()}-0${date?.getUTCMonth() + 1}-${date?.getUTCDate() + 1}`;
    } else if (date.getUTCDate() + 1 < 10) {
      return `${date?.getUTCFullYear()}-${date?.getUTCMonth() + 1}-0${date?.getUTCDate() + 1}`;
    }
    return `${date?.getUTCFullYear()}-${date?.getUTCMonth() + 1}-0${date?.getUTCDate() + 1}`;
  }

  private convertAvailabilityDateFormats(ads: IAvailability[]){
    ads.forEach(av => {
      av.dateTo = new Date(av.dateTo ?? '');
      av.dateFrom = new Date(av.dateFrom ?? '');
    })
    return ads;
  }
}
