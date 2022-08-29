import { Injectable } from '@angular/core';
import { catchError } from "rxjs/operators";
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { map, Observable, throwError } from 'rxjs';
import { Advertisement } from "../../models/advertisement";
import { AuthService } from "../auth/auth.service";
import { GeocodingService } from "../geocoding/geocoding.service";
import { AdvertisementDetail } from "../../models/advertisement-detail";
import { environment } from "../../../environments/environment";
import {Category} from "../../models/category";

@Injectable({
  providedIn: 'root'
})
export class AdvertisementService {
  private baseUrl: string;
  private baseUrlCategory: string;
  private headers: HttpHeaders

  constructor(private http: HttpClient, private authService: AuthService, private geocodingService: GeocodingService) {
    this.baseUrl = environment.baseUrl + "Advertisement/";
    this.baseUrlCategory = environment.baseUrl + "Category/";
    this.headers = authService.getHeaders();
  }

  getAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getOwnAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>(this.baseUrl + "own", { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getSavedAdvertisements(): Observable<Advertisement[]>{
    return this.http.get<Advertisement[]>(this.baseUrl + "saved", { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getAdvertisementDetailById(id: number): Observable<AdvertisementDetail>{
    return this.http.get<AdvertisementDetail>(this.baseUrl + id, { headers: this.headers})
      .pipe(
        map((response: AdvertisementDetail) => this.setDates(response)),
        catchError(this.handleError));
  }

  createAdvertisement(ad: AdvertisementDetail): Observable<number>{
    return this.http.post<any>(this.baseUrl, ad, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  updateAdvertisement(ad: AdvertisementDetail): Observable<number>{
    return this.http.put<any>(this.baseUrl, ad, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<any>(this.baseUrlCategory, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  setDates(ad: AdvertisementDetail): AdvertisementDetail{
    ad.availabilities.forEach(av => {
      av.dateTo = new Date(av.dateTo);
      av.dateFrom = new Date(av.dateFrom);
    })
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
