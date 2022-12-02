import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {environment} from "../../../environments/environment";
import {AuthService} from "../auth/auth.service";
import {BehaviorSubject, map, Observable} from "rxjs";
import {INotification} from "../../models/notification";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private baseUrl: string;
  private headers: HttpHeaders;

  newNotiCount = new BehaviorSubject<number | null>(null);

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Notification/";
    this.headers = authService.getHeaders();

    if (authService.isLoggedIn()){
      this.getNewNotificationCount();
    } else {
      this.newNotiCount.next(null);
    }
  }

  newNotificationCount(): Observable<number | null> {
    return this.newNotiCount.asObservable();
  }

  getNewNotificationCount() {
    this.http.get<number>(this.baseUrl + 'new', { headers: this.headers})
      .subscribe(count => {
        if (count > 0)
        {
          this.newNotiCount.next(count);
        } else {
          this.newNotiCount.next(null);
        }
      });
  }

  getAllNotifications(): Observable<INotification[]> {
    return this.http.get<INotification[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((notifications: INotification[]) => {
          return notifications;
        }));
  }

  setSeenNotifications(notifications: INotification[]) {
    let notificationIds = this.getNewNotificationIds(notifications);
    this.http.post(this.baseUrl, notificationIds,{ headers: this.headers})
      .subscribe((response) =>{
        this.getNewNotificationCount();
      });
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
