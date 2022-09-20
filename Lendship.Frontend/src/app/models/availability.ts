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

  constructor(id: number, dateFrom: string, dateTo: string) {
    this.id = id;
    this.dateFrom = dateFrom;
    this.dateTo = dateTo;
  }
}
