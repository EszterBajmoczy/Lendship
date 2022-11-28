import { Component, OnInit } from '@angular/core';
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import { AdvertisementList } from "../../models/advertisementList";
import { Router} from "@angular/router";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-advertisements-page',
  templateUrl: './advertisements-page.component.html',
  styleUrls: ['./advertisements-page.component.scss']
})
export class AdvertisementsPageComponent implements OnInit {
  baseUrl = environment.baseUrl;
  modeOwn = "Own";
  modeSaved = "Saved";
  ownAds: AdvertisementList | undefined;
  ownAdsLoading = true;
  savedAds: AdvertisementList | undefined;
  savedAdsLoading = true;
  searchOwnWithOutPaging = "";
  searchSavedWithOutPaging = "";

  showOwn: boolean = true;
  showSaved: boolean = false;

  constructor(private adService: AdvertisementService, private router: Router) {
    this.adService.getOwnAdvertisements("")
      .subscribe(ads => {
        if(ads !== undefined){
          this.ownAds = ads;
        }
        this.ownAdsLoading = false;
      })
  }

  ngOnInit(): void {
  }

  loadSavedAdvertisements(){
    this.showSaved = ! this.showSaved;
    if(this.savedAds === undefined){
      this.adService.getSavedAdvertisements("")
        .subscribe( data => {
          if(data !== undefined){
            this.savedAds = data
          }
          this.savedAdsLoading = false;
        });
    }
  }

  cardClicked(id: number) {
    this.router.navigate(['advertisement', id]);
  }

  searchSavedAdvertisements(advertisements: AdvertisementList) {
    this.savedAds = advertisements;
  }

  searchOwnAdvertisements(advertisements: AdvertisementList) {
    this.ownAds = advertisements;
  }

  updateOwnSearchWithOutPaging(search: string) {
    this.searchOwnWithOutPaging = search;
  }

  updateSavedSearchWithOutPaging(search: string) {
    this.searchSavedWithOutPaging = search;
  }
}
