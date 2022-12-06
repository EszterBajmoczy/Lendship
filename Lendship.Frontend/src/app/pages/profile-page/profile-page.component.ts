import { Component, OnInit } from '@angular/core';
import { EvaluationAdvertiser, EvaluationLender} from "../../models/evaluations";
import {ActivatedRoute} from "@angular/router";
import {UserService} from "../../services/user/user.service";
import {UserDetail} from "../../models/user-detail";
import {AuthService} from "../../services/auth/auth.service";
import {environment} from "../../../environments/environment";
import {DateHandlerService} from "../../services/date-handler/date-handler.service";

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.scss']
})
export class ProfilePageComponent implements OnInit {
  baseUrl = environment.baseUrl;
  baseImage = environment.baseImage;
  user: UserDetail | undefined;
  isOwn: Boolean = true;

  evaluationsAdvertiser: EvaluationAdvertiser[] | undefined;
  evaluationsLender: EvaluationLender[] | undefined;

  showAdvertiserEvaluations: boolean = false;
  showLenderEvaluations: boolean = false;

  constructor(
    private userService: UserService,
    authService: AuthService,
    private dateHandlerService: DateHandlerService,
    activatedRoute: ActivatedRoute) {
    activatedRoute.params.subscribe( params => {
      let id = params['id'];
      if(id != null){
        this.userService.getOtherUser(id)
          .subscribe(user => {
            console.log("other");
            console.log(user);
            this.user = user;
            this.isOwn = false;
            this.loadAdvertiserEvaluations();
          });
      } else {
        this.userService.getUser()
          .subscribe(user => {
            console.log(user);
            this.user = user;
            this.isOwn = true;
            this.loadAdvertiserEvaluations();
          });
      }
    });
  }

  loadAdvertiserEvaluations(){
    console.log("A")
    console.log(this.evaluationsAdvertiser)
    console.log(this.user)
    this.showAdvertiserEvaluations = !this.showAdvertiserEvaluations;
    if(this.evaluationsAdvertiser === undefined && this.user !== undefined){
      console.log("B")

      this.userService.getEvaluationAdvertiserUser(this.user.id)
        .subscribe(evaluations => {
          console.log(evaluations);
          evaluations.map(ev => {
            ev.creationFormatted = this.dateHandlerService.convertDateToString(new Date(ev.creation));
          });
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
          evaluations.map(ev => {
            ev.creationFormatted = this.dateHandlerService.convertDateToString(new Date(ev.creation));
          });
          this.evaluationsLender = evaluations;
        });
    }
  }

  ngOnInit(): void {
  }

}
