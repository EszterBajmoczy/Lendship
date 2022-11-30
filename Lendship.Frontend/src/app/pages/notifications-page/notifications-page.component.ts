import { Component, OnInit } from '@angular/core';
import {NotificationService} from "../../services/notification/notification.service";
import {INotification} from "../../models/notification";
import {Router} from "@angular/router";
import {DateHandlerService} from "../../services/date-handler/date-handler.service";

@Component({
  selector: 'app-notifications-page',
  templateUrl: './notifications-page.component.html',
  styleUrls: ['./notifications-page.component.scss']
})
export class NotificationsPageComponent implements OnInit {
  loading = true;
  notifications = new Array<INotification>()

  constructor(
    private notificationService: NotificationService,
    private dateHandlerService: DateHandlerService,
    private router: Router) {
    notificationService.getAllNotifications()
      .subscribe((notifications) => {
        notifications.forEach(noti => {
          noti.reservationDateFromString = this.dateHandlerService.convertDateToString(noti.reservationDateFrom);
          noti.reservationDateToString = this.dateHandlerService.convertDateToString(noti.reservationDateTo);
        })
        this.notifications = notifications;
        this.loading = false;
        console.log(notifications);

        this.notificationService.setSeenNotifications(notifications)
          .subscribe((result) => {
            console.log(result);
          });
    })
  }

  ngOnInit(): void {
  }

  notificationClicked() {
    this.router.navigateByUrl("reservations");
  }
}
