import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {AuthService} from "../auth/auth.service";
import {map, Observable } from "rxjs";
import {INotification} from "../../models/notification";

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
        }));
  }

  getAllNotifications(): Observable<INotification[]> {
    return this.http.get<INotification[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }));
  }

  setSeenNotifications(notifications: INotification[]): Observable<INotification[]> {
    let notificationIds = this.getNewNotificationIds(notifications);
    return this.http.post<INotification[]>(this.baseUrl, notificationIds,{ headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }));
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
}
