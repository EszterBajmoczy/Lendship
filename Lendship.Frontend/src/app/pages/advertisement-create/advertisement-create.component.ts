import {Component, OnInit} from '@angular/core';
import {UntypedFormBuilder, UntypedFormGroup, Validators} from "@angular/forms";
import {LocationValidator} from "../../shared/valid-location";
import {GeocodingService} from "../../services/geocoding/geocoding.service";
import {Availability, IAvailability} from "../../models/availability";
import {NgbDate, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {AdvertisementService} from "../../services/advertisement/advertisement.service";
import {AdvertisementDetail} from "../../models/advertisement-detail";
import {ActivatedRoute, Router} from "@angular/router";
import {FileUploadService} from "../../services/file-upload/file-upload.service";
import {Category} from "../../models/category";
import {environment} from "../../../environments/environment";
import {DateHandlerService} from "../../services/date-handler/date-handler.service";
import {User} from "../../models/user";


@Component({
  selector: 'app-advertisement-create',
  templateUrl: './advertisement-create.component.html',
  styleUrls: ['./advertisement-create.component.scss'],
  providers: [LocationValidator]
})
export class AdvertisementCreateComponent implements OnInit {
  baseUrl = environment.baseUrl;
  advertisement: AdvertisementDetail | undefined;
  availabilities = Array<Availability>();
  privateUserList = Array<User>();
  privateUserToBe: User | undefined;
  isPrivate = false;

  categories = Array<Category>();
  categoryKeyword = 'name';

  newImageNames = new Array<string>();
  newImages = new Array<File>();

  reserveFrom: NgbDate | undefined;
  reserveTo: NgbDate | undefined;

  error: string = "";

  mode: string = "";
  id: number = 0;
  advertisementForm: UntypedFormGroup;

  submitting = false;

  constructor(
    private formBuilder: UntypedFormBuilder,
    private locationValidator: LocationValidator,
    private geoCodingService: GeocodingService,
    private modalService: NgbModal,
    private advertisementService: AdvertisementService,
    private fileUploadService: FileUploadService,
    private ngbDateHandlerService: DateHandlerService,
    private router: Router,
    activatedRoute: ActivatedRoute)
  {
    activatedRoute.params.subscribe( params => {
      let advertisementId = params['advertisementId'];
      if(advertisementId !== undefined){
        advertisementService.getAdvertisementDetailById(advertisementId)
          .subscribe((ad) => {
              this.advertisement = ad;
              this.availabilities = this.transformAvailabilities(ad.availabilities);
              this.privateUserList = ad.privateUsers;
              this.isPrivate = !ad.isPublic;
              this.mode = "Edit";
              this.initializeForm();
              console.log(ad);
            }
          )
      } else {
        this.mode = "Create";
      }
    });

    advertisementService.getCategories()
      .subscribe((categories) => {
        this.categories = categories;
      })

    this.advertisementForm = this.formBuilder.group({
      id: [0],
      isService: [false],
      title: ["", [Validators.required]],
      price: [0, [Validators.pattern("^[0-9]*$")]],
      credit: [0, [Validators.pattern("^[0-9]*$")]],
      deposit: [0, [Validators.pattern("^[0-9]*$")]],
      description: ["", [Validators.required]],
      instructionManual: [""],
      location: ["", [Validators.required], [this.locationValidator.exists.bind(this.locationValidator)]],
      latitude: [0],
      longitude: [0],
      isPublic: [!this.isPrivate],
      category: [null, [Validators.required]],
      availabilities: [],
      privateUsers: []
    });
  }

  transformAvailabilities(availabilities: IAvailability[]): Availability[]{
    let result = new Array<Availability>();
    availabilities.forEach(av => {
      let dateFrom = this.ngbDateHandlerService.convertDateToString(av.dateFrom);
      let dateTo = this.ngbDateHandlerService.convertDateToString(av.dateTo);
      result.push(new Availability(av.id, dateFrom, dateTo));
    });
    return result;
  }

  initializeForm(){
    this.advertisementForm = this.formBuilder.group({
      id: [this.advertisement?.id],
      isService: [this.advertisement?.isService],
      title: [this.advertisement?.title, [Validators.required]],
      price: [this.advertisement?.price, [Validators.pattern("^[0-9]*$")]],
      credit: [this.advertisement?.credit, [Validators.pattern("^[0-9]*$")]],
      deposit: [this.advertisement?.deposit, [Validators.pattern("^[0-9]*$")]],
      description: [this.advertisement?.description, [Validators.required]],
      instructionManual: [this.advertisement?.instructionManual],
      location: [this.advertisement?.location, [Validators.required], [this.locationValidator.exists.bind(this.locationValidator)]],
      latitude: [this.advertisement?.latitude],
      longitude: [this.advertisement?.longitude],
      isPublic: [this.advertisement?.isPublic],
      category: [this.advertisement?.category, [Validators.required]],
      availabilities: [this.advertisement?.availabilities],
      privateUsers: [this.advertisement?.privateUsers]
    });
  }

  ngOnInit(): void {
  }

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

  set privateUsers(value: User[]) {
    this.advertisementForm.get("privateUsers")?.setValue(value);
  }

  set category(value: any) {
    this.advertisementForm.get("category")?.setValue(value);
  }

  onSubmit(){
    console.log(this.advertisementForm.value);
    if(this.advertisementForm.invalid){
      return;
    }
    if(this.availabilities.length == 0){
      this.error = "No availability selected.";
      return;
    }
    let previousImages = this.advertisement !== undefined ? this.advertisement.imageLocations.length : 0;
    if(this.newImages.length == 0 && previousImages <= 0){
      this.error = "At least one photo should be uploaded.";
      return;
    }

    this.geoCodingService.getLatLong(this.advertisementForm.get("location")?.value)
      .subscribe(data => {
        this.latitude = data.results[0].geometry.location.lat;
        this.longitude = data.results[0].geometry.location.lng;
        this.availability = this.availabilities;
        this.privateUsers = this.privateUserList;

        let categoryName = this.category?.value.name;
        if(categoryName === undefined){
          this.category = new Category(0, this.category?.value);
        }
        console.log(this.advertisementForm.value);
        this.save(this.advertisementForm.value);
      })
  }

  private save(data: any) {
    if(this.mode === "Create"){
      this.submitting = true;
      this.advertisementService.createAdvertisement(data)
        .subscribe(response => {
          this.uploadFiles(response);
        });
    } else {
      this.submitting = true;
      this.advertisementService.updateAdvertisement(data)
        .subscribe(response => {
          this.uploadFiles(this.advertisement!!.id);
        });
    }
  }

  open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result
      .then((result) => {
        if(result == "Add" && this.reserveFrom !== undefined && this.reserveTo !== undefined){
          let dateFrom = this.ngbDateHandlerService.convertNgbDateToString(this.reserveFrom);
          let dateTo = this.ngbDateHandlerService.convertNgbDateToString(this.reserveTo);
          this.availabilities.push(new Availability(0, dateFrom, dateTo));
          this.error = "";
        } else if(result == "AddPrivateUser" && this.privateUserToBe !== undefined){
          console.log("AddPrivateUser");
          this.privateUserList.push(this.privateUserToBe);
          this.privateUserToBe = undefined;
          console.log(this.privateUserList)
        }
      })
      .catch((res) => {});
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

  addFile(newFile: File) {
    this.newImages.push(newFile);
    this.newImageNames.push(newFile.name);
  }

  uploadFiles(id: number) {
    if(this.newImages != null && this.newImages.length > 0){
      this.fileUploadService.upload(id, this.newImages)
        .subscribe(
        (event: any) => {
          this.router.navigateByUrl('home');
        }
      );
    } else {
      this.router.navigateByUrl('home');
    }
  }

  removeNewImage(i: number) {
    this.newImageNames.splice(i, 1);
    this.newImages.splice(i, 1);
  }

  removeImage(i: number) {
    if(this.advertisement != null || this.advertisement !== undefined){
      let element = this.advertisement?.imageLocations.splice(i, 1)[0];
      this.fileUploadService.deleteFile(this.advertisement?.id, element);
    }
  }

  showPrivateWindow() {
    this.isPrivate = !this.isPrivate;
  }

  addPrivateUser(user: User) {
    this.privateUserToBe = user;
  }

  removePrivateUser(i: number) {
    this.privateUserList.splice(i, 1);
  }
}
