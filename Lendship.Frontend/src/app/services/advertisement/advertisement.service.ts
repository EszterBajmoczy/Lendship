import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AdvertisementList } from "../../models/advertisementList";
import { AuthService } from "../auth/auth.service";
import { AdvertisementDetail } from "../../models/advertisement-detail";
import { environment } from "../../../environments/environment";
import {Category} from "../../models/category";

@Injectable({
  providedIn: 'root'
})
export class AdvertisementService {
  private readonly baseUrl: string;
  private readonly baseUrlCategory: string;
  private readonly headers: HttpHeaders

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Advertisement/";
    this.baseUrlCategory = environment.baseUrl + "Category/";
    this.headers = authService.getHeaders();
  }

  getAdvertisements(search: string): Observable<AdvertisementList>{
    return this.http.get<AdvertisementList>(this.baseUrl + search, { headers: this.headers});
  }

  getOwnAdvertisements(search: string): Observable<AdvertisementList>{
    return this.http.get<AdvertisementList>(this.baseUrl + "own/" + search, { headers: this.headers});
  }

  getSavedAdvertisements(search: string): Observable<AdvertisementList>{
    return this.http.get<AdvertisementList>(this.baseUrl + "saved/" + search, { headers: this.headers});
  }

  getAdvertisementDetailById(id: number): Observable<AdvertisementDetail>{
    return this.http.get<AdvertisementDetail>(this.baseUrl + id, { headers: this.headers})
      .pipe(
        map((response: AdvertisementDetail) => this.setDates(response)));
  }

  createAdvertisement(ad: AdvertisementDetail): Observable<number>{
    return this.http.post<any>(this.baseUrl, ad, { headers: this.headers});
  }

  updateAdvertisement(ad: AdvertisementDetail): Observable<number>{
    return this.http.put<any>(this.baseUrl, ad, { headers: this.headers});
  }

  deleteAdvertisementById(id: number): Observable<number>{
    return this.http.delete<any>(this.baseUrl + id, { headers: this.headers});
  }

  isAdvertisementSaved(id: number): Observable<boolean>{
    return this.http.get<any>(this.baseUrl + "saved/" + id, { headers: this.headers});
  }

  saveAdvertisementById(id: number): Observable<number>{
    return this.http.post<any>(this.baseUrl + "saved/" + id, { headers: this.headers});
  }

  removeSavedAdvertisementById(id: number): Observable<number>{
    return this.http.delete<any>(this.baseUrl + "saved/" + id, { headers: this.headers});
  }

  getCategories(): Observable<Category[]> {
    return this.http.get<any>(this.baseUrlCategory, { headers: this.headers});
  }

  setDates(ad: AdvertisementDetail): AdvertisementDetail{
    ad.availabilities.forEach(av => {
      av.dateTo = new Date(av.dateTo);
      av.dateFrom = new Date(av.dateFrom);
    })
    return ad;
  }
}
