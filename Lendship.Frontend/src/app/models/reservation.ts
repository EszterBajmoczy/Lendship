import {NgbDate} from "@ng-bootstrap/ng-bootstrap";
import {DateHandlerService} from "../services/date-handler/date-handler.service";

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
    let dateHandler = new DateHandlerService();

    this.id = id;
    this.reservationState = reservationsState;
    this.dateFrom = dateHandler.convertNgbDateToString(dateFrom);
    this.dateTo = dateHandler.convertNgbDateToString(dateTo);
  }
}


