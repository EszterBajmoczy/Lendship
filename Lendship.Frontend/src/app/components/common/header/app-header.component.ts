import { Component } from '@angular/core';
import { AuthService } from "../../../services/auth/auth.service";
import { Router} from "@angular/router";
import {NotificationService} from "../../../services/notification/notification.service";
import {INotification} from "../../../models/notification";
import {environment} from "../../../../environments/environment";

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  providers: [AuthService]
})

export class AppHeaderComponent {
  baseUrl = environment.baseUrl;
  name: string = "Login";
  nameUrl: string = "login";
  image: string = "";
  isLoggedIn: boolean = false;
  notificationCount: number = 0;
  notifications = new Array<INotification>();

  constructor(private authService: AuthService, private notificationService: NotificationService, private router: Router) {
    let user = this.authService.getUserName();
    let img = this.authService.getProfileImage();

    if(user) {
      this.name = user;
      this.nameUrl = "profile";
      this.image = img;
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
