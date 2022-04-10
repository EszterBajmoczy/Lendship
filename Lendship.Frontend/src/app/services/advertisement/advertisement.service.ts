import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders} from '@angular/common/http';
import {map, Observable, throwError} from 'rxjs';
import { Advertisement } from "../../models/advertisement";
import { AuthService } from "../auth/auth.service";
import { GeocodingService} from "../geocoding/geocoding.service";
import {AdvertisementDetail} from "../../models/advertisement-detail";

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

  setLocations(ads: Advertisement[]){
    ads.forEach((ad) => {
      this.geocodingService.getAddress(ad.latitude, ad.longitude)
        .subscribe(location => ad.location = location["city"]);
    });
    console.log(ads);
    return ads;
  }

  setLocation(ad: AdvertisementDetail): AdvertisementDetail{
    this.geocodingService.getAddress(ad.latitude, ad.longitude)
      .subscribe(location => ad.location = location["city"]);
    return ad;
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
