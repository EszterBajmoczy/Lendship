import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

export interface IAvailability {
  id: number;
  dateFrom: Date;
  dateTo: Date;
}

export class Availability {
  id: number;
  dateFrom: string;
  dateTo: string;

  constructor(id: number, dateFrom: NgbDate, dateTo: NgbDate) {
    this.id = id;
    this.dateFrom = this.dateToString(dateFrom);
    this.dateTo = this.dateToString(dateTo);
  }

  dateToString(date: NgbDate): string {
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
