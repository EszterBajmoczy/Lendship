import { Component, OnInit } from '@angular/core';
import { PasswordMatchingValidator} from "../../shared/password-matching";
import { FormBuilder, Validators} from "@angular/forms";
import { AuthService} from "../../services/auth/auth.service";
import { Router} from "@angular/router";

@Component({
  selector: 'app-registration-page',
  templateUrl: './registration-page.component.html',
  styleUrls: ['./registration-page.component.scss'],
  providers: [AuthService]
})
export class RegistrationPageComponent implements OnInit {

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  registrationForm = this.formBuilder.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    location: ['', [Validators.required]],
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

  get password() {
    return this.registrationForm.get("password");
  }

  get confirmPassword() {
    return this.registrationForm.get("confirmPassword");
  }

  onSubmit(){
    //TODO
    if(this.registrationForm.invalid){
      return;
    }

    this.authService.register(this.registrationForm.value);
  }
}
