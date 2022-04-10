import { Component, OnInit } from '@angular/core';
import { ActivatedRoute} from "@angular/router";
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import {AdvertisementDetail} from "../../models/advertisement-detail";

@Component({
  selector: 'app-advertisement-info-page',
  templateUrl: './advertisement-info-page.component.html',
  styleUrls: ['./advertisement-info-page.component.scss']
})
export class AdvertisementInfoPageComponent implements OnInit {
  id: number = -1;
  ad: AdvertisementDetail | undefined;

  constructor(private adService: AdvertisementService, activatedRoute: ActivatedRoute) {
    activatedRoute.params.subscribe( params => {
      this.id = params['id'];
      this.adService.getAdvertisementDetailById(this.id)
        .subscribe(ad => {
          console.log(ad);
          this.ad = ad;
        });
    });
  }

  ngOnInit(): void {
  }

}
