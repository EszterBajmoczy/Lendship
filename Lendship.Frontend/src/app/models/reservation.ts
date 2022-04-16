import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

export interface IReservation {
  id: number | undefined;
  reservationState: number | undefined;
  dateFrom: Date | string | undefined;
  dateTo: Date | string | undefined;
}

export class Reservation implements IReservation {
  id: number;
  reservationState: number;
  dateFrom: string;
  dateTo: string;

  constructor(id: number, reservationsState: number, dateFrom: NgbDate, dateTo: NgbDate) {
    this.id = id;
    this.reservationState = reservationsState;
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

