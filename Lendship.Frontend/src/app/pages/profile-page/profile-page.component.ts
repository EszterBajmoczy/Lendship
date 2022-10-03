import { Component, OnInit } from '@angular/core';
import { EvaluationAdvertiser, EvaluationLender} from "../../models/evaluations";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../services/user/user.service";
import {UserDetail} from "../../models/user-detail";
import {AuthService} from "../../services/auth/auth.service";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {
  image
  user: UserDetail | undefined;

  evaluationsAdvertiser: EvaluationAdvertiser[] | undefined;
  evaluationsLender: EvaluationLender[] | undefined;

  showAdvertiserEvaluations: boolean = false;
  showLenderEvaluations: boolean = false;

  constructor(private userService: UserService, authService: AuthService, activatedRoute: ActivatedRoute) {
    this.image = authService.getProfileImage();
    activatedRoute.params.subscribe( params => {
      let id = params['id'];
      if(id != null){
        this.userService.getOtherUser(id)
          .subscribe(user => {
            console.log("other" +user);
            this.user = user;
          });
      } else {
        this.userService.getUser()
          .subscribe(user => {
            console.log(user);
            this.user = user;
            this.loadAdvertiserEvaluations();
          });
      }
    });
  }

  loadAdvertiserEvaluations(){
    this.showAdvertiserEvaluations = !this.showAdvertiserEvaluations;
    if(this.evaluationsAdvertiser === undefined && this.user !== undefined){
      this.userService.getEvaluationAdvertiserUser(this.user.id)
        .subscribe(evaluations => {
          console.log(evaluations);
          this.evaluationsAdvertiser = evaluations;
        });
    }
  }

  loadLenderEvaluations(){
    this.showLenderEvaluations = !this.showLenderEvaluations;
    if(this.evaluationsLender === undefined && this.user !== undefined) {
      console.log(this.user?.id);
      this.userService.getEvaluationLenderUser(this.user.id)
        .subscribe(evaluations => {
          console.log(evaluations);
          this.evaluationsLender = evaluations;
        });
    }
  }

  ngOnInit(): void {
  }

}
