import {NgbDate} from "@ng-bootstrap/ng-bootstrap";
import {Advertisement} from "./advertisement";

export interface IReservationDetail {
  id: number;
  advertisement: Advertisement;
  reservationState: number;
  comment: string;
  admittedByAdvertiser: boolean;
  admittedByLender: boolean;
  dateFrom: Date;
  dateTo: Date;
}

export class ReservationDetail {
  id: number;
  advertisement: Advertisement;
  reservationState: number;
  comment: string;
  admittedByAdvertiser: boolean;
  admittedByLender: boolean;
  dateFrom: string;
  dateTo: string;

  constructor(id: number, ad: Advertisement, reservationsState: number, comment: string, admittedByAdvertiser: boolean, admittedByLender: boolean, dateFrom: NgbDate, dateTo: NgbDate) {
    this.id = id;
    this.advertisement = ad;
    this.reservationState = reservationsState;
    this.comment = comment;
    this.admittedByAdvertiser = admittedByAdvertiser;
    this.admittedByLender = admittedByLender;
    this.dateFrom = this.dateToString(dateFrom);
    this.dateTo = this.dateToString(dateTo);
  }

  dateToString(date: NgbDate) {
    if (date.month < 10 && date.day < 10) {
      return `${date.year}-0${date.month}-0${date.day}`;
    } else if (date.month < 10) {
      return `${date?.year}-0${date?.month}-${date?.day}`;
    } else if (date.day < 10) {
      return `${date?.year}-${date?.month}-0${date?.day}`;
    }
    return `${date?.year}-${date?.month}-0${date?.day}`;
  }
}

