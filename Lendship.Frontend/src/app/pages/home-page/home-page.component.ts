import { Component, OnInit } from '@angular/core';
import { AuthService} from "../../services/auth/auth.service";
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import { Advertisement } from "../../models/advertisement";

@Component({
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  isLoggedIn: boolean;
  ads: Advertisement[] | undefined;

  constructor(private adService: AdvertisementService, authService: AuthService) {

    this.isLoggedIn = authService.isLoggedIn();
    if(this.isLoggedIn) {
      this.loadAdvertisements();
    }
  }

  ngOnInit(): void {
  }

  loadAdvertisements(){
    this.adService.getAdvertisements()
      .subscribe(
        data => this.ads = data);
  }
}
