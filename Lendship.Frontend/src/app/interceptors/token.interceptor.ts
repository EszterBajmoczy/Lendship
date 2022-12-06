import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor, HttpErrorResponse, HTTP_INTERCEPTORS
} from '@angular/common/http';
import {BehaviorSubject, Observable, throwError, switchMap, take, filter} from 'rxjs';
import {AuthService} from "../services/auth/auth.service";
import {catchError} from "rxjs/operators";
import {environment} from "../../environments/environment";
import {ErrorService} from "../services/error/error.service";
import {Router} from "@angular/router";

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  private shouldLogout = false;
  private isRefreshing = false;
  private token: BehaviorSubject<any> = new BehaviorSubject<any>(
    null
  );

  constructor(private authService: AuthService,
              private errorService: ErrorService,
              private router: Router) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if(!request.url.includes(environment.geocodingBaseUrl)){
      if (this.authService.getAccessToken()) {
        request = this.addToken(request, this.authService.getAccessToken());
      }
    }

    return next.handle(request).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse && error.error == "Invalid refresh token") {
          return throwError(error);
        } else if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        } else if (error instanceof HttpErrorResponse && (error.status === 406)) {
          return throwError(error);
        } else {
          this.errorService.setError(error);
          this.router.navigateByUrl('error');
          return throwError(error);
        }
      })
    );
  }

  private addToken(request: HttpRequest<any>, token: string | null) {
    if(token != null){
      return request.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`,
        },
      });
    } else {
      return request;
    }
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if(this.shouldLogout){
      this.authService.logout();
      this.shouldLogout = false;
    } else {
      this.shouldLogout = true;

    }
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.token.next(null);

      return this.authService.refreshToken().pipe(
        switchMap((token: any) => {
          this.isRefreshing = false;
          this.token.next(token.token);
          return next.handle(this.addToken(request, token.token));
        })
      );
    } else {
      return this.token.pipe(
        filter((token: null) => token != null),
        take(1),
        switchMap((jwt) => {
          return next.handle(this.addToken(request, jwt));
        })
      );
    }
  }
}


export const tokenInterceptor = {
  provide: HTTP_INTERCEPTORS,
  useClass: TokenInterceptor,
  multi: true
};
