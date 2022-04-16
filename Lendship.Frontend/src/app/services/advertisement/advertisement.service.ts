import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {map, Observable, throwError} from 'rxjs';
import { Advertisement } from "../../models/advertisement";
import { AuthService } from "../auth/auth.service";
import { GeocodingService} from "../geocoding/geocoding.service";
import {AdvertisementDetail} from "../../models/advertisement-detail";
import {LoginResponse} from "../../models/response-login";

@Injectable({
  providedIn: 'root'
})
export class AdvertisementService {
  headers: HttpHeaders

  constructor(private http: HttpClient, private authService: AuthService, private geocodingService: GeocodingService) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.authService.getAccessToken()}`
    });
  }

  getAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>("https://localhost:44377/Advertisement", { headers: this.headers})
      .pipe(
        map((response: Advertisement[]) => this.setLocations(response)),
        catchError(this.handleError));
  }

  getOwnAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>("https://localhost:44377/Advertisement/own", { headers: this.headers})
      .pipe(
        map((response: Advertisement[]) => this.setLocations(response)),
        catchError(this.handleError));
  }

  getSavedAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>("https://localhost:44377/Advertisement/saved", { headers: this.headers})
      .pipe(
        map((response: Advertisement[]) => this.setLocations(response)),
        catchError(this.handleError));
  }

  getAdvertisementDetailById(id: number): Observable<AdvertisementDetail>{
    return this.http.get<AdvertisementDetail>("https://localhost:44377/Advertisement/" + id, { headers: this.headers})
      .pipe(
        map((response: AdvertisementDetail) => this.setLocation(response)),
        catchError(this.handleError));
  }

  createAdvertisement(ad: AdvertisementDetail): Observable<number>{
    return this.http.post<any>("https://localhost:44377/Advertisement/", ad, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  setLocations(ads: Advertisement[]){
    ads.forEach((ad) => {
      ad.location = "MockLocation";
      /*this.geocodingService.getAddress(ad.latitude, ad.longitude)
        .subscribe(location => ad.location = location["city"]);*/
    });
    console.log(ads);
    return ads;
  }

  setLocation(ad: AdvertisementDetail): AdvertisementDetail{
    ad.availabilities.forEach(av => {
      av.dateTo = new Date(av.dateTo);
      av.dateFrom = new Date(av.dateFrom);
    })
    this.geocodingService.getAddress(ad.latitude, ad.longitude)
      .subscribe(location => ad.location = location["city"]);
    return ad;
  }

  private handleError(error: HttpErrorResponse) {
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
