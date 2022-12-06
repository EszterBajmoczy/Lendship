import {NgbDate} from "@ng-bootstrap/ng-bootstrap";
import {Advertisement, AdvertisementList} from "./advertisementList";
import {User} from "./user";
import {DateHandlerService} from "../services/date-handler/date-handler.service";

export interface IReservationDetail {
  id: number;
  advertisement: Advertisement;
  reservationState: string;
  comment: string;
  admittedByAdvertiser: boolean;
  admittedByLender: boolean;
  user: User;
  dateFrom: Date;
  dateTo: Date;
  dateFromString: string;
  dateToString: string;
  dateFromNgbDate: NgbDate;
  dateToNgbDate: NgbDate;
}

export class ReservationDetail {
  id: number;
  advertisement: AdvertisementList;
  reservationState: string;
  comment: string;
  admittedByAdvertiser: boolean;
  admittedByLender: boolean;
  dateFrom: string;
  dateTo: string;

  constructor(id: number, ad: AdvertisementList, reservationsState: string, comment: string, admittedByAdvertiser: boolean, admittedByLender: boolean, dateFrom: NgbDate, dateTo: NgbDate) {
    let dateHandler = new DateHandlerService();

    this.id = id;
    this.advertisement = ad;
    this.reservationState = reservationsState;
    this.comment = comment;
    this.admittedByAdvertiser = admittedByAdvertiser;
    this.admittedByLender = admittedByLender;
    this.dateFrom = dateHandler.convertNgbDateToString(dateFrom);
    this.dateTo = dateHandler.convertNgbDateToString(dateTo);
  }
}


