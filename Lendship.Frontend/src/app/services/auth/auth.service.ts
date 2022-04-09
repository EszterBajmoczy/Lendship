import { Injectable } from '@angular/core';
import { LoginUser} from "../../models/login-user";
import { RegisterUser} from "../../models/registration-user";
import { LoginResponse} from "../../models/response-login";
import { HttpClient, HttpErrorResponse} from '@angular/common/http';
import { throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { LocalStorageService} from "../localstorage/localstorage.service";
import { JWTTokenService} from "../jwttoken/jwttoken.service";
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(
    private http: HttpClient,
    private localstorageService: LocalStorageService,
    private tokenService: JWTTokenService,
    private router: Router ) { }

  public login(userData: LoginUser) {
    const loginCall = this.http.post<LoginResponse>("https://localhost:44377/Authentication/login",userData)
      .pipe(
        catchError(this.handleError));

    loginCall.subscribe(response => {
      this.saveLoginData(response)

      this.router.navigate(['home'])
        .then(() => {
          window.location.reload();
        });
    });
  }

  public register(userData: RegisterUser) {
    const registerCall = this.http.post<LoginResponse>("https://localhost:44377/Authentication/register",userData)
      .pipe(
        catchError(this.handleError));

    registerCall.subscribe(response => {
      this.saveLoginData(response)
      this.login(userData);
    });
  }

  private saveLoginData(resp: LoginResponse) {
    this.localstorageService.set("ACCESS_TOKEN", resp.token);
    this.localstorageService.set("REFRESH_TOKEN", resp.token);
    this.tokenService.setToken(resp.token);
  }

  public isLoggedIn() {
    return localStorage.getItem('ACCESS_TOKEN') !== null;
  }

  public getUserName() : string {
    return this.tokenService.getUserName() ?? "";
  }

  public getAccessToken() : string {
    return this.localstorageService.get("ACCESS_TOKEN") ?? "";
  }

  public getRefreshToken() : string {
    return this.localstorageService.get("REFRESH_TOKEN") ?? "";
  }

  public logout() {
    localStorage.removeItem('ACCESS_TOKEN');
    localStorage.removeItem('REFRESH_TOKEN');
    this.tokenService.removeToken();
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
