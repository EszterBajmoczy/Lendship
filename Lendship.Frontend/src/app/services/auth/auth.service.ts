import { Injectable } from '@angular/core';
import { LoginUser} from "../../models/login-user";
import { RegisterUser} from "../../models/registration-user";
import { LoginResponse} from "../../models/response-login";
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { of, tap} from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LocalStorageService} from "../localstorage/localstorage.service";
import { JWTTokenService} from "../jwttoken/jwttoken.service";
import { Router } from '@angular/router';
import {environment} from "../../../environments/environment";
import {FileUploadService} from "../file-upload/file-upload.service";

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly baseUrl: string;
  private readonly JWT_TOKEN = "JWT_TOKEN";
  private readonly REFRESH_TOKEN = "REFRESH_TOKEN";

  constructor(
    private http: HttpClient,
    private localstorageService: LocalStorageService,
    private tokenService: JWTTokenService,
    private router: Router )
  {
    this.baseUrl = environment.baseUrl + "Authentication";
  }

  public login(userData: LoginUser) {
    const loginCall = this.http.post<LoginResponse>(this.baseUrl + "/login",userData);

    loginCall.subscribe(response => {
      this.saveLoginData(response)

      this.router.navigate(['home'])
        .then(() => {
          window.location.reload();
        });
    });
  }

  public register(userData: RegisterUser) {
    return this.http.post<LoginResponse>(this.baseUrl + "/register", userData);
  }

  public loginData(userData: LoginResponse) {
    this.saveLoginData(userData);
    this.router.navigate(['home'])
      .then(() => {
        window.location.reload();
      });
  }


  private saveLoginData(resp: LoginResponse) {
    this.localstorageService.set("ACCESS_TOKEN", resp.token);
    if(resp.refreshToken != null){
      this.localstorageService.set("REFRESH_TOKEN", resp.refreshToken);
    }
    this.tokenService.setToken(resp.token);
    this.localstorageService.set("PROFILE_IMG", resp.image);

  }

  public isLoggedIn() {
    return localStorage.getItem('ACCESS_TOKEN') !== null;
  }

  public getUserName() : string {
    return this.tokenService.getUserName() ?? "";
  }

  public getUserId() : string {
    return this.tokenService.getUserId() ?? "";
  }

  public getAccessToken() : string {
    return this.localstorageService.get("ACCESS_TOKEN") ?? "";
  }

  public getRefreshToken() : string {
    return this.localstorageService.get("REFRESH_TOKEN") ?? "";
  }

  public getProfileImage() : string {
    return this.localstorageService.get("PROFILE_IMG") ?? "";
  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
    localStorage.removeItem('REFRESH_TOKEN');
    localStorage.removeItem('PROFILE_IMG');
    this.tokenService.removeToken();
    if (this.router.url === "/home"){
      location.reload();
    } else {
      this.router.navigateByUrl('home');
    }
  }

  refreshToken() {
    return this.http.post<LoginResponse>(this.baseUrl + "/refresh", {
      refreshToken: this.getRefreshToken(),
    })
      .pipe(
        tap((tokens) => {
          this.saveLoginData(tokens);
        }),
        catchError((error) => {
          this.logout();
          return of(false);
        })
      );
  }

  getHeaders(): HttpHeaders{
    return new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${this.getAccessToken()}`
    });
  }
}
