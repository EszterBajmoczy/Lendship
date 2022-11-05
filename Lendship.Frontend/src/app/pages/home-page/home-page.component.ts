import { Component, OnInit } from '@angular/core';
import { AuthService} from "../../services/auth/auth.service";
import { AdvertisementService} from "../../services/advertisement/advertisement.service";
import { Advertisement } from "../../models/advertisement";
import { Router } from "@angular/router";
import {environment} from "../../../environments/environment";
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {LocationValidator} from "../../shared/valid-location";
import {GeocodingService} from "../../services/geocoding/geocoding.service";
import {Category} from "../../models/category";

@Component({
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
  providers: [LocationValidator]
})
export class HomePageComponent implements OnInit {
  baseUrl = environment.baseUrl
  isLoggedIn: boolean;
  ads: Advertisement[] | undefined;
  searchForm: UntypedFormGroup;
  property = "Property";
  service = "Service";

  categories = Array<Category>();
  categoryKeyword = 'name';

  error: string | undefined = undefined;

  constructor(
    private adService: AdvertisementService,
    private formBuilder: UntypedFormBuilder,
    private locationValidator: LocationValidator,
    private geoCodingService: GeocodingService,
    private router: Router,
    authService: AuthService) {

    this.isLoggedIn = authService.isLoggedIn();
    if(this.isLoggedIn) {
      this.loadAdvertisements("");
    }

    adService.getCategories()
      .subscribe((categories) => {
        this.categories = categories;
      })

    this.searchForm = this.formBuilder.group({
      advertisementType: [this.property],
      credit: [false],
      cash: [true],
      category: [""],
      city: [""],
      distance: [10],
      word: [""]
    });
  }

  ngOnInit(): void {
  }

  loadAdvertisements(word: string = ""){
    this.adService.getAdvertisements(word)
      .subscribe(data => {
          this.ads = data;
          console.log(data)
        });
  }

  cardClicked(id: number) {
    this.router.navigate(['advertisement', id]);
  }

  getBasicSearchString(): string {
    let result = "?advertisementType=" + this.searchForm.get("advertisementType")?.value;
    result += "&credit=" + this.searchForm.get("credit")?.value;
    result += "&cash=" + this.searchForm.get("cash")?.value;
    result += "&sortBy=" + (<HTMLInputElement>document.getElementById('sortBy')).value;

    let category = this.category?.value.name;
    let word = this.word?.value.name;
    if (category !== "" && category !== undefined && category !== null )
      result += "&category=" + this.category?.value.name;
    if (word !== "" && word !== undefined && word !== null )
      result += "&word=" + this.word?.value.name;

    return result;
  }

  searchSubmit() {
    let search = this.getBasicSearchString();
    console.log(this.searchForm.value);

    if (this.searchForm.get("city")?.value !== "") {
      this.geoCodingService.getLatLong(this.searchForm.get("city")?.value)
        .subscribe(data => {
          //TODO ha nem tudja feloldani
          if (data.status === "ZERO_RESULTS") {
            this.city = "";
            this.error = "Invalid city";
          } else {
            this.error = undefined;
            console.log(data);
            search += "&latitude=" + data.results[0].geometry.location.lat;
            search += "&longitude=" + data.results[0].geometry.location.lng;
            search += "&distance=" + this.searchForm.get("distance")?.value;
            this.loadAdvertisements(search);
          }
        })
    } else {
      this.loadAdvertisements(search);
    }
  }

  isProperty(): Boolean {
    return this.searchForm.get("advertisementType")?.value === this.property;
  }

  isService(): Boolean {
    return this.searchForm.get("advertisementType")?.value === this.service;
  }

  changeAdvertisementType(type: string) {
    this.searchForm.get("advertisementType")?.setValue(type);
  }

  get category() {
    return this.searchForm.get("category");
  }

  get word() {
    return this.searchForm.get("word");
  }

  set city(value: string) {
    this.searchForm.get("city")?.setValue(value);
  }
}
