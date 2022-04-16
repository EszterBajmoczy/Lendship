import { Component, OnInit } from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {LocationValidator} from "../../shared/valid-location";
import {GeocodingService} from "../../services/geocoding/geocoding.service";
import {Availability} from "../../models/availability";
import {NgbDate, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AdvertisementService} from "../../services/advertisement/advertisement.service";
import {Router} from "@angular/router";


@Component({
  selector: 'app-advertisement-create',
  templateUrl: './advertisement-create.component.html',
  styleUrls: ['./advertisement-create.component.scss'],
  providers: [LocationValidator]
})
export class AdvertisementCreateComponent implements OnInit {
  availabilities = Array<Availability>();

  reserveFrom: NgbDate | undefined;
  reserveTo: NgbDate | undefined;

  error: string = "";

  id: number = 0;

  constructor(
    private formBuilder: FormBuilder,
    private locationValidator: LocationValidator,
    private geoCodingService: GeocodingService,
    private modalService: NgbModal,
    private advertisementService: AdvertisementService,
    private router: Router) {

  }

  ngOnInit(): void {
  }

  advertisementForm = this.formBuilder.group({
    title: ['', [Validators.required]],
    price: [0, [Validators.pattern("^[0-9]*$")]],
    credit: [0, [Validators.pattern("^[0-9]*$")]],
    deposit: [0, [Validators.pattern("^[0-9]*$")]],
    description: ['', [Validators.required]],
    instructionManual: [],
    location: ['', [Validators.required], [this.locationValidator.exists.bind(this.locationValidator)]],
    latitude: [],
    longitude: [],
    isPublic: [true],
    category: ['', [Validators.required]],
    availabilities: []
  });

  get title() {
    return this.advertisementForm.get("title");
  }

  get price() {
    return this.advertisementForm.get("price");
  }

  get credit() {
    return this.advertisementForm.get("credit");
  }

  get deposit() {
    return this.advertisementForm.get("deposit");
  }

  get description() {
    return this.advertisementForm.get("description");
  }

  get category() {
    return this.advertisementForm.get("category");
  }

  get location() {
    return this.advertisementForm.get("location");
  }

  set latitude(value: number) {
    this.advertisementForm.get("latitude")?.setValue(value);
  }

  set longitude(value: number) {
    this.advertisementForm.get("longitude")?.setValue(value);
  }

  set availability(value: Availability[]) {
    this.advertisementForm.get("availabilities")?.setValue(value);
  }

  set category(value: any) {
    this.advertisementForm.get("category")?.setValue(value);
  }

  onSubmit(){
    if(this.advertisementForm.invalid){
      return;
    }
    if(this.availabilities.length == 0){
      this.error = "No availability selected.";
      return;
    }
    this.geoCodingService.getLatLong(this.advertisementForm.get("location")?.value)
      .subscribe(data => {
        this.latitude = data['latt'];
        this.longitude = data['longt'];
        this.availability = this.availabilities;
        this.category = "Kert";
        //TODO call create
        console.log("!!!!!!!!!!");
        console.log(this.availabilities);
        console.log(this.advertisementForm.value);
        this.advertisementService.createAdvertisement(this.advertisementForm.value)
          .subscribe(response => {
            //TODO handle success
            this.id = response;
            console.log(response);
            //this.router.navigateByUrl('home');
          });
      })
  }

  open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      if(result == "Add" && this.reserveFrom !== undefined && this.reserveTo !== undefined){
        this.availabilities.push(new Availability(0, this.reserveFrom, this.reserveTo));
        this.error = "";
      }
    });
  }

  reserveDateFrom(from: NgbDate) {
    this.reserveFrom = from;
  }

  reserveDateTo(to: NgbDate) {
    this.reserveTo = to;
  }

  removeAvailability(i: number) {
    this.availabilities.splice(i, 1);
  }
}
