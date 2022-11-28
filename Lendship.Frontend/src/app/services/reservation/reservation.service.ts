import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders} from '@angular/common/http';
import {AuthService} from "../auth/auth.service";
import {map, Observable } from "rxjs";
import {Reservation} from "../../models/reservation";
import {IAvailability} from "../../models/availability";
import {environment} from "../../../environments/environment";
import {IReservationDetail} from "../../models/reservation-detail";
import {IReservationBasic} from "../../models/reservation-basic";
import {ITransactionOperation} from "../../models/transaction-operation";
import {IReservationToken, QRToken} from "../../models/reservation-token";
import {DateHandlerService} from "../date-handler/date-handler.service";

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private readonly baseUrl: string;
  private readonly headers: HttpHeaders;

  constructor(
    private http: HttpClient,
    private dateHandler: DateHandlerService,
    private authService: AuthService) {
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

  getReservationToken(resId: number, closing: boolean): Observable<IReservationToken> {
    return this.http.get<IReservationToken>(this.baseUrl + "reservationtoken/" + resId + "/" + closing, { headers: this.headers});
  }

  validateReservationToken(token: string): Observable<ITransactionOperation> {
    var data = new QRToken(token);
    return this.http.post<ITransactionOperation>(this.baseUrl + "reservationtoken",  data, { headers: this.headers});
  }

  isReservationClosed(reservationId: number): Observable<boolean> {
    return this.http.get<boolean>(this.baseUrl + "closed/" + reservationId, { headers: this.headers});
  }

  private convertReservationDateFormats(res: IReservationDetail[]){
    res.forEach(r => {
      r.dateFrom = new Date(r.dateFrom ?? '');
      r.dateTo = new Date(r.dateTo ?? '');
      r.dateFromString = this.dateHandler.convertDateToString(r.dateFrom);
      r.dateToString = this.dateHandler.convertDateToString(r.dateTo);
      r.dateFromNgbDate = this.dateHandler.convertDateToNgbDate(r.dateFrom, true);
      r.dateToNgbDate = this.dateHandler.convertDateToNgbDate(r.dateTo, false);
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
}
