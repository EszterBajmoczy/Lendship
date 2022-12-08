import { Component, OnInit } from '@angular/core';
import { passwordMatchingValidator} from "../../shared/password-matching";
import {FormGroup, UntypedFormBuilder, Validators} from "@angular/forms";
import { AuthService} from "../../services/auth/auth.service";
import { LocationValidator} from "../../shared/valid-location";
import { GeocodingService} from "../../services/geocoding/geocoding.service";
import {FileUploadService} from "../../services/file-upload/file-upload.service";
import {Router} from "@angular/router";
import {catchError} from "rxjs/operators";

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss'],
  providers: [AuthService, LocationValidator]
})
export class RegistrationPageComponent implements OnInit {
  submitting = false;
  newImageName = "";
  newImage: File | undefined;
  error = "";

  constructor(
    private formBuilder: UntypedFormBuilder,
    private authService: AuthService,
    private router: Router,
    private locationValidator: LocationValidator,
    private geoCodingService: GeocodingService,
    private fileUploadService: FileUploadService) { }

  ngOnInit(): void {
  }

  registrationForm: FormGroup = this.formBuilder.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    location: ['', [Validators.required], [this.locationValidator.exists.bind(this.locationValidator)]],
    latitude: [],
    longitude: [],
    password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])"))]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.pattern(new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])"))]],
  }, {validators: [passwordMatchingValidator()]});

  get name() {
    return this.registrationForm.get("name");
  }

  get email() {
    return this.registrationForm.get("email");
  }

  get location() {
    return this.registrationForm.get("location");
  }

  set latitude(value: number) {
    this.registrationForm.get("latitude")?.setValue(value);
  }

  set longitude(value: number) {
    this.registrationForm.get("longitude")?.setValue(value);
  }

  get password() {
    return this.registrationForm.get("password");
  }

  get confirmPassword() {
    return this.registrationForm.get("confirmPassword");
  }

  onSubmit(){
    if(this.registrationForm.invalid){
      return;
    }

    this.geoCodingService.getLatLong(this.registrationForm.get("location")?.value)
      .subscribe(data => {
        this.latitude = data.results[0].geometry.location.lat;
        this.longitude = data.results[0].geometry.location.lng;

        this.authService.register(this.registrationForm.value)
          .pipe(
            catchError(error => {
              console.log(error.error.message);
              this.error = error.error.message;
              this.submitting = false;
              throw(error);
            })
          )
          .subscribe(response => {
            if (this.newImage !== undefined){
              this.fileUploadService.uploadProfile(response.token, this.newImage)
                .subscribe(resp => {
                  console.log(resp);
                  response.image = resp.path;
                  this.authService.loginData(response)
                });
            } else {
              this.authService.loginData(response)
            }
        });
        this.submitting = true;
      })
  }

  addFile(img: File) {
    this.newImageName = img.name;
    this.newImage = img;
  }

  removeNewImage() {
    this.newImageName = "";
    this.newImage = undefined;
  }
}
