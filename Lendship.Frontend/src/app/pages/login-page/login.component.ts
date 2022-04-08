import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators} from "@angular/forms";
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { AuthService} from "../../services/auth/auth.service";
import { LocalStorageService} from "../../services/localstorage/localstorage.service";
import { JWTTokenService} from "../../services/jwttoken/jwttoken.service";

//https://medium.com/techiediaries-com/angular-9-8-authentication-form-angular-formbuilder-formgroup-and-validators-example-with-node-f91729db006f
//authgard

@Component({
  selector: 'app-login-page',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  providers: [LocalStorageService, JWTTokenService]
})
export class LoginComponent implements OnInit {
  mymsg: string ="";
  constructor(private formBuilder: FormBuilder, private authService: AuthService, private localstorageService: LocalStorageService, private jwtService: JWTTokenService, private router: Router ) {
    //this.usernameService.usernameObservable.subscribe(u => this.mymsg = u);
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
    //TODO
    console.log(this.loginForm.value);
    if(this.loginForm.invalid){
      return;
    }
    this.authService.login(this.loginForm.value)
      .subscribe(resp => {
        this.router.navigate(['home'])
          .then(() => {
            window.location.reload();
          });
      });
  }

}
