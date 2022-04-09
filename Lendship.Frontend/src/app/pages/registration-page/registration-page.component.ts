import { Component, OnInit } from '@angular/core';
import { PasswordMatchingValidator} from "../../shared/password-matching";
import { FormBuilder, Validators} from "@angular/forms";
import { AuthService} from "../../services/auth/auth.service";
import { LocationValidator} from "../../shared/valid-location";
import { GeocodingService} from "../../services/geocoding/geocoding.service";

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss'],
  providers: [AuthService, LocationValidator]
})
export class RegistrationPageComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private locationValidator: LocationValidator, private geoCodingService: GeocodingService) { }

  ngOnInit(): void {
  }

  registrationForm = this.formBuilder.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    location: ['', [Validators.required], [this.locationValidator.exists.bind(this.locationValidator)]],
    latitude: [],
    longitude: [],
    password: ['', [Validators.required, Validators.minLength(6), Validators.pattern(new RegExp(/^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+="!_()']).*$/i))]],
    confirmPassword: ['', [Validators.required, Validators.minLength(6), Validators.pattern(new RegExp(/^(?=.{8,})(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[@#$%^&+="!_()']).*$/i))]],
  }, { validators: PasswordMatchingValidator });

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
        console.log("haho " + data);
        this.latitude = data['latt'];
        this.longitude = data['longt'];

        this.authService.register(this.registrationForm.value);
      })

  }
}