import { Component } from '@angular/core';
import { AuthService } from "../../../services/auth/auth.service";
import { Router} from "@angular/router";
import {NotificationService} from "../../../services/notification/notification.service";
import {INotification} from "../../../models/notification";
import {environment} from "../../../../environments/environment";
import {Conversation} from "../../../models/conversation";
import {ConversationService} from "../../../services/conversation/conversation.service";
import {BehaviorSubject, Observable} from "rxjs";

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
  messagesCount: Observable<number | null>;
  notificationCount: Observable<number | null>;

  constructor(
    private authService: AuthService,
    private conversationService: ConversationService,
    private notificationService: NotificationService,
    private router: Router) {
    let user = this.authService.getUserName();
    let img = this.authService.getProfileImage();

    if(user) {
      this.name = user;
      this.nameUrl = "profile";
      this.image = img;
      this.isLoggedIn = true;
    }

    if (this.isLoggedIn) {
      this.messagesCount = conversationService.newMessageCount();
      this.notificationCount = notificationService.newNotificationCount();
    } else {
      this.messagesCount = new BehaviorSubject<number | null>(null);
      this.notificationCount = new BehaviorSubject<number | null>(null);
    }
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
    this.notificationService.getNewNotificationCount();
  }
}
