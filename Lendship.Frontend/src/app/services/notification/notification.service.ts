import { Injectable } from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {AuthService} from "../auth/auth.service";
import {map, Observable, throwError} from "rxjs";
import {INotification} from "../../models/notification";
import {catchError} from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl: string;
  private headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Notification/";
    this.headers = authService.getHeaders();
  }

  getNewNotifications(): Observable<INotification[]> {
    return this.http.get<INotification[]>(this.baseUrl + 'new', { headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }),
        catchError(this.handleError));
  }

  getAllNotifications(): Observable<INotification[]> {
    return this.http.get<INotification[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }),
        catchError(this.handleError));
  }

  setSeenNotifications(notifications: INotification[]): Observable<INotification[]> {
    let notificationIds = this.getNewNotificationIds(notifications);
    return this.http.post<INotification[]>(this.baseUrl, notificationIds,{ headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }),
        catchError(this.handleError));
  }

  private getNewNotificationIds(notifications: INotification[]) {
    let ids = new Array<number>();
    notifications.forEach((notification) => {
      if (notification.new){
        ids.push(notification.id);
      }
    })
    return ids;
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
