import { Component } from '@angular/core';
import { AuthService } from "../../../services/auth/auth.service";
import { Router} from "@angular/router";
import {NotificationService} from "../../../services/notification/notification.service";
import {INotification} from "../../../models/notification";

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  providers: [AuthService]
})

export class AppHeaderComponent {
  name: string = "Login";
  nameUrl: string = "login";
  isLoggedIn: boolean = false;
  notificationCount: number = 0;
  notifications = new Array<INotification>();

  constructor(private authService: AuthService, private notificationService: NotificationService, private router: Router) {
    let user = this.authService.getUserName();
    console.log(user);
    if(user) {
      this.name = user;
      this.nameUrl = "profile";
      this.isLoggedIn = true;
    }

    notificationService.getNewNotifications().subscribe((notifications) => {
      this.notificationCount = notifications.length;
      this.notifications = notifications;
    });
  }

  ngOnInit(): void {

  }

  logout() {
    this.authService.logout();
    console.log("logged out.")
    this.router.navigate(['home'])
      .then(() => {
        window.location.reload();
      });
  }

  removeNewNotifications() {
    this.notificationCount = 0;
  }
}
