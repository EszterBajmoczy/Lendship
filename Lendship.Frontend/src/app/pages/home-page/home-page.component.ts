import { Component, OnInit } from '@angular/core';
import { AuthService} from "../../services/auth/auth.service";
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import { AdvertisementList } from "../../models/advertisementList";
import { Router } from "@angular/router";
import {environment} from "../../../environments/environment";
import {UntypedFormBuilder} from "@angular/forms";
import {LocationValidator} from "../../shared/valid-location";
import {GeocodingService} from "../../services/geocoding/geocoding.service";

@Component({
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
  providers: [LocationValidator]
})
export class HomePageComponent implements OnInit {
  baseUrl = environment.baseUrl
  isLoggedIn: boolean;
  mode = "GetAll";
  searchWithOutPaging = "";

  adList: AdvertisementList | undefined;

  constructor(
    private adService: AdvertisementService,
    private formBuilder: UntypedFormBuilder,
    private geoCodingService: GeocodingService,
    private router: Router,
    authService: AuthService) {

    this.isLoggedIn = authService.isLoggedIn();
    if(this.isLoggedIn) {
      this.adService.getAdvertisements("")
        .subscribe(data => {
          this.adList = data;
          console.log(data)
        });
    }
  }

  ngOnInit(): void {
  }

  cardClicked(id: number) {
    this.router.navigate(['advertisement', id]);
  }

  searchAdvertisements(advertisements: AdvertisementList) {
    this.adList = advertisements;
  }

  updateSearchWithOutPaging(search: string) {
    this.searchWithOutPaging = search;
  }
}
