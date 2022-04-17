import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from "rxjs";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  //geocode.xyz
  private readonly baseUrl: string;
  private readonly apiKey: string;

  public constructor(private http: HttpClient) {
    this.baseUrl = environment.geocodingBaseUrl;
    this.apiKey = environment.geocodingApiKey;
  }

  getLatLong(address: string) : Observable<any> {
    const params = {
      auth: this.apiKey,
      locate: address,
      json: '1'
    }

    return this.http.get(this.baseUrl, {params});
  }

  getAddress(lat: number, long: number) : Observable<any>{
    const params = {
      auth: this.apiKey,
      locate: lat + ',' + long,
      json: '1'
    }
    return this.http.get(this.baseUrl, {params});
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
