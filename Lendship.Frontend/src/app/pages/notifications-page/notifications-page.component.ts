import { Component, OnInit } from '@angular/core';
import {NotificationService} from "../../services/notification/notification.service";
import {INotification} from "../../models/notification";
import {Router} from "@angular/router";

@Component({
  selector: 'app-notifications-page',
  templateUrl: './notifications-page.component.html',
  styleUrls: ['./notifications-page.component.scss']
})
export class NotificationsPageComponent implements OnInit {
  notifications = new Array<INotification>()

  constructor(private notificationService: NotificationService, private router: Router) {
    notificationService.getAllNotifications()
      .subscribe((notifications) => {
        notifications.forEach(noti => {
          noti.reservationDateFromString = this.formatDate(noti.reservationDateFrom);
          noti.reservationDateToString = this.formatDate(noti.reservationDateTo);
        })
        this.notifications = notifications;
        console.log(notifications);

        this.notificationService.setSeenNotifications(notifications)
          .subscribe((result) => {
            console.log(result);
          });
    })
  }

  ngOnInit(): void {
  }

  formatDate(d: Date): string {
    let date = new Date(d);
    let result = `${date.getFullYear()}-`;
    if((date.getUTCMonth() + 1) < 10) {
      result += `0${(date.getUTCMonth() + 1)}-`;
    } else {
      result += `${(date.getUTCMonth() + 1)}-`;
    }
    if(date.getUTCDate() < 10){
      result += `0${date.getUTCDate()}`;
    } else {
      result += `${date.getUTCDate()}`;
    }
    return result;
  }

  notificationClicked() {
    this.router.navigateByUrl("reservations");
  }
}
