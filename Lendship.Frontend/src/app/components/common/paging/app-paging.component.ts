import {Component, EventEmitter, Input, Output} from '@angular/core';
import {AdvertisementList} from "../../../models/advertisementList";
import {AdvertisementService} from "../../../services/advertisement/advertisement.service";

@Component({
  selector: 'app-paging',
  templateUrl: './app-paging.component.html',
  styleUrls: ['./app-paging.component.scss']
})

export class AppPagingComponent {
  @Input() mode: String = "";
  @Input() pages: Number | undefined;
  @Input() search = "";
  @Output() advertisements = new EventEmitter<AdvertisementList>();

  currentPage = 0;

  constructor(private adService: AdvertisementService) { console.log(this.pages) }

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

  paging(page: number) {
    console.log(this.currentPage);
    console.log(page);
    this.currentPage = page;
    let searchString = this.search;

    if (searchString.length == 0){
      searchString += "?page=" + page;
    } else {
      searchString += "&page=" + page;
    }
    console.log(searchString);

    this.loadAdvertisements(searchString);
  }
}
