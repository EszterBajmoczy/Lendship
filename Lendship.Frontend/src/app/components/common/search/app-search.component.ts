import {Component, EventEmitter, Input, Output} from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup} from "@angular/forms";
import {AdvertisementList} from "../../../models/advertisementList";
import {AdvertisementService} from "../../../services/advertisement/advertisement.service";
import {Category} from "../../../models/category";
import {GeocodingService} from "../../../services/geocoding/geocoding.service";

@Component({
  selector: 'app-search',
  templateUrl: './app-search.component.html',
  styleUrls: ['./app-search.component.scss']
})

export class AppSearchComponent {
  @Input() mode: String = "";
  @Output() advertisements = new EventEmitter<AdvertisementList>();
  @Output() searchWithOutPaging = new EventEmitter<string>();

  searchForm: UntypedFormGroup;
  property = "Property";
  service = "Service";

  categories = Array<Category>();
  categoryKeyword = 'name';

  error: string | undefined = undefined;

  constructor(
    private adService: AdvertisementService,
    private formBuilder: UntypedFormBuilder,
    private geoCodingService: GeocodingService
  ) {

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

  loadAdvertisements(search: string = ""){
    if (this.mode === "GetAll"){
      this.adService.getAdvertisements(search)
        .subscribe(data => {
          this.advertisements.emit(data);
          console.log(data)
        });
    } else if (this.mode === "Saved") {
      this.adService.getSavedAdvertisements(search)
        .subscribe(data => {
          this.advertisements.emit(data);
          console.log(data)
        });
    } else if (this.mode === "Own") {
      this.adService.getOwnAdvertisements(search)
        .subscribe(data => {
          this.advertisements.emit(data);
          console.log(data)
        });
    }
  }

  searchSubmit() {
    let search = this.getBasicSearchString();
    this.searchWithOutPaging.emit(search);
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

  getBasicSearchString(): string {
    let result = "?advertisementType=" + this.searchForm.get("advertisementType")?.value;
    result += "&credit=" + this.searchForm.get("credit")?.value;
    result += "&cash=" + this.searchForm.get("cash")?.value;

    let category = this.category?.value.name;
    let word = this.word?.value;
    console.log(word);
    if (category !== "" && category !== undefined && category !== null )
      result += "&category=" + this.category?.value.name;
    if (word !== "" && word !== undefined && word !== null )
      result += "&word=" + this.word?.value;

    return result;
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
