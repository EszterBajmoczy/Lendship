import { Injectable } from '@angular/core';
import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

@Injectable({
  providedIn: 'root'
})
export class DateHandlerService {

  constructor() { }

  convertDateToString(date: Date): string {
    date.setDate(date.getDate() + 1);
    let result = `${date.getFullYear()}.`;
    if((date.getUTCMonth() + 1) < 10) {
      result += `0${(date.getUTCMonth() + 1)}.`;
    } else {
      result += `${(date.getUTCMonth() + 1)}.`;
    }
    if(date.getUTCDate() < 10){
      result += `0${date.getUTCDate()}`;
    } else {
      result += `${date.getUTCDate()}`;
    }
    return result;
  }

  convertDateToNgbDate(date: Date, isFromDate: boolean): NgbDate{
    if(isFromDate){
      return new NgbDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate())
    }

    let d = new Date(date);
    d.setHours(d.getHours() + 48)
    return new NgbDate(d.getUTCFullYear(), d.getUTCMonth() + 1, d.getUTCDate())
  }

  convertNgbDateToString(date: NgbDate): string {
    let d = new Date();
    d.setUTCFullYear(date.year);
    d.setMonth(date.month-1);
    d.setDate(date.day);

    let result = `${d.getFullYear()}.`;
    if((d.getUTCMonth() + 1) < 10) {
      result += `0${(d.getUTCMonth() + 1)}.`;
    } else {
      result += `${(d.getUTCMonth() + 1)}.`;
    }
    if(d.getUTCDate() < 10){
      result += `0${d.getUTCDate()}`;
    } else {
      result += `${d.getUTCDate()}`;
    }
    return result;
  }
}
