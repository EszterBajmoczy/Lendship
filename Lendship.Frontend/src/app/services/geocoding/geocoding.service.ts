import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError} from "rxjs/operators";
import {Observable, throwError} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  //positionstack
  //baseUrl = "http://api.positionstack.com/v1/"
  //api_key = "d57005395dc614d7f5551e2841d2a80d";

  //geocode.xyz
  baseUrl = "https://geocode.xyz/";
  api_key = "318774527394438479612x29999";

  public constructor(private http: HttpClient) {
  }

  getLatLong(address: string) : Observable<any> {
    const params = {
      auth: this.api_key,
      locate: address,
      json: '1'
    }

    return this.http.get(this.baseUrl, {params});
  }

  getAddress(lat: number, long: number) : Observable<any>{
    const params = {
      auth: this.api_key,
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
