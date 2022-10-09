import { Component, OnInit } from '@angular/core';
import {ErrorService} from "../../services/error/error.service";
import {HttpErrorResponse} from "@angular/common/http";
import {Router} from "@angular/router";

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss']
})
export class ErrorPageComponent implements OnInit {
  error: HttpErrorResponse | null;

  constructor(errorService: ErrorService, router: Router) {
    this.error = errorService.getError();
    if(this.error === undefined){
      router.navigateByUrl('home');
    }
  }

  ngOnInit(): void {
  }

}
