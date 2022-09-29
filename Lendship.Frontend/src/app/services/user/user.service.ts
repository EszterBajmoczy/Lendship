import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { AuthService } from "../auth/auth.service";
import { UserDetail } from "../../models/user-detail";
import { GeocodingService} from "../geocoding/geocoding.service";
import {EvaluationAdvertiser, EvaluationLender} from "../../models/evaluations";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private readonly baseUrl: string;
  private readonly headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService, private geoCodingService: GeocodingService) {
    this.baseUrl = environment.baseUrl;
    this.headers = authService.getHeaders();
  }

  getUser(): Observable<UserDetail>{
    return this.http.get<UserDetail>(this.baseUrl + "Profile/", { headers: this.headers})
      .pipe(
        map((response: UserDetail) => this.setLocation(response)));
  }

  getOtherUser(id: string): Observable<UserDetail>{
    return this.http.get<UserDetail>(this.baseUrl + "Profile/" + id, { headers: this.headers})
      .pipe(
        map((response: UserDetail) => this.setLocation(response)));
  }

  getEvaluationAdvertiserUser(id: string): Observable<EvaluationAdvertiser[]>{
    return this.http.get<EvaluationAdvertiser[]>(this.baseUrl + "EvaluationAdvertiser/" + id, { headers: this.headers});
  }

  getEvaluationLenderUser(id: string): Observable<EvaluationLender[]>{
    return this.http.get<EvaluationLender[]>(this.baseUrl + "/EvaluationLender/" + id, { headers: this.headers});
  }

  setLocation(user: UserDetail): UserDetail{
    this.geoCodingService.getAddress(user.latitude, user.longitude)
      .subscribe(location => {
        user.location = location["city"]
      });
    return user;
  }
}
