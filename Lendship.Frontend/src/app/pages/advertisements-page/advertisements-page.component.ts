import { Component, OnInit } from '@angular/core';
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import { Advertisement } from "../../models/advertisement";
import { GeocodingService} from "../../services/geocoding/geocoding.service";

@Component({
  selector: 'app-advertisements-page',
  templateUrl: './advertisements-page.component.html',
  styleUrls: ['./advertisements-page.component.scss']
})
export class AdvertisementsPageComponent implements OnInit {
  ownAds: Advertisement[] | undefined;
  savedAds: Advertisement[] | undefined;

  showOwn: boolean = true;
  showSaved: boolean = false;

  constructor(private adService: AdvertisementService, private geocodingService: GeocodingService) {
    this.adService.getOwnAdvertisements()
      .subscribe(ads => this.ownAds = ads)
  }

  ngOnInit(): void {
  }

  loadSavedAdvertisements(){
    this.showSaved = ! this.showSaved;
    if(this.savedAds === undefined){
      this.adService.getSavedAdvertisements()
        .subscribe(
          data => this.savedAds = data);
    }
  }

  cardClicked(id: number) {

  }
}
