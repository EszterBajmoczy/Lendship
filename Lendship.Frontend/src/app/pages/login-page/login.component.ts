import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, Validators} from "@angular/forms";
import { Router } from '@angular/router';
import { AuthService} from "../../services/auth/auth.service";

//https://medium.com/techiediaries-com/angular-9-8-authentication-form-angular-formbuilder-formgroup-and-validators-example-with-node-f91729db006f
//authgard

@Component({
  selector: 'app-login-page',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  submitting = false;

  constructor(private formBuilder: UntypedFormBuilder, private authService: AuthService, private router: Router ) {
  }

  ngOnInit(): void {
  }

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  get email() {
    return this.loginForm.get("email");
  }

  get password() {
    return this.loginForm.get("password");
  }

  onSubmit(){
    if(this.loginForm.invalid){
      return;
    }
    this.authService.login(this.loginForm.value);
    this.submitting = true;
  }

  registrationClicked(){
    this.router.navigateByUrl('registration');
  }
}
