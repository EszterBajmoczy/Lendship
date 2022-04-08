import { Component } from '@angular/core';
import { AuthService } from "../../../services/auth/auth.service";
import { Router} from "@angular/router";

@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  providers: [AuthService]
})

export class AppHeaderComponent {
  name: string = "Login";
  nameUrl: string = "login";
  isLoggedIn: boolean = false;

  constructor(private authService: AuthService, private router: Router) {
    let user = this.authService.getUserName();
    console.log(user);
    if(user) {
      this.name = user;
      this.nameUrl = "profile";
      this.isLoggedIn = true;

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
}
