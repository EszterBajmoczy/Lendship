import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import { Observable, throwError } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
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
    console.log(lat +"-"+ long);
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
