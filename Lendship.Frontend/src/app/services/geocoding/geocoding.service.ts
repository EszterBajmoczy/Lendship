import { Injectable } from '@angular/core';
import {HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class GeocodingService {
  private readonly baseUrl: string;
  private readonly apiKey: string;

  public constructor(private http: HttpClient) {
    this.baseUrl = environment.geocodingBaseUrl;
    this.apiKey = environment.geocodingApiKey;
  }

  getLatLong(address: string) : Observable<any> {
    let url = this.baseUrl + "?address=" + address + "&key=" + this.apiKey;
    return this.http.get(url);
  }

  getAddress(lat: number, long: number) : Observable<any>{
    const params = {
      auth: this.apiKey,
      locate: lat + ',' + long,
      json: '1'
    }

    let url = this.baseUrl + "?latlng=" + lat + "," + long + "&key=" + this.apiKey;
    return this.http.get(url);
  }
}
