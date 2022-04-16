import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { NgbCalendar, NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { ReservationService} from "../../../services/reservation/reservation.service";
import {IAvailability} from "../../../models/availability";

@Component({
  selector: 'app-reservation-popup',
  templateUrl: './reservation-popup.component.html',
  styleUrls: ['./reservation-popup.component.scss']
})
export class ReservationPopupComponent implements OnInit {
  @Input() id: number = 0;
  @Input() availabilities: IAvailability[] | undefined;
  reserved: IAvailability[] | undefined;

  hoveredDate: NgbDate | null = null;

  fromDate: NgbDate | null = null;
  toDate: NgbDate | null = null;

  @Output() reserveFrom = new EventEmitter<NgbDate>();
  @Output() reserveTo = new EventEmitter<NgbDate>();

  constructor(private calendar: NgbCalendar, private reservationService: ReservationService) {
  }

  ngOnInit(): void {
    this.reservationService.getReservationForAdvertisement(this.id)
      .subscribe(response => {
        this.reserved = response;
      })
  }

  isDisabled = (date: NgbDate, current?: {year: number, month: number}) => {
    let notAvailable = this.notAvailable(date);

    let alreadyReserved = this.alreadyReserved(date);

    return notAvailable || alreadyReserved;
  }

  notAvailable(date: NgbDate){
    let result = true;
    this.availabilities?.forEach( av => {
      let from = new NgbDate(av.dateFrom.getUTCFullYear(), av.dateFrom.getUTCMonth() + 1, av.dateFrom.getUTCDate());
      let to = new NgbDate(av.dateTo.getUTCFullYear(), av.dateTo.getUTCMonth() + 1, av.dateTo.getUTCDate()+2);

      if(date.after(from) && date.before(to)) {
        result = false;
      }
    });
    return result;
  }

  alreadyReserved = (date: NgbDate, current?: {year: number, month: number}) => {
    let reserved = false;
    this.reserved?.forEach( av => {
      let from = new NgbDate(av.dateFrom.getUTCFullYear(), av.dateFrom.getUTCMonth() + 1, av.dateFrom.getUTCDate());
      let to = new NgbDate(av.dateTo.getUTCFullYear(), av.dateTo.getUTCMonth() + 1, av.dateTo.getUTCDate()+2);

      if(date.after(from) && date.before(to)) {
        reserved = true;
      }
    });
    return reserved;
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      this.reserveFrom.emit(date);
    } else if (this.fromDate && !this.toDate && date.after(this.fromDate) && this.isAllEnabledBetween(date)) {
      this.toDate = date;
      this.reserveTo.emit(date);
    } else {
      this.toDate = null;
      this.fromDate = date;
      this.reserveFrom.emit(date);
    }
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) ||
      this.isHovered(date);
  }

  isAllEnabledBetween(date: NgbDate): Boolean{
    let result = false;
    this.availabilities?.forEach( av => {
      let from = new NgbDate(av.dateFrom.getUTCFullYear(), av.dateFrom.getUTCMonth() + 1, av.dateFrom.getUTCDate());
      let to = new NgbDate(av.dateTo.getUTCFullYear(), av.dateTo.getUTCMonth() + 1, av.dateTo.getUTCDate()+2);

      if(this.fromDate?.after(from) && date.before(to)) {
        result = true;
      }
    });

    this.reserved?.forEach(av => {
      let from = new NgbDate(av.dateFrom.getUTCFullYear(), av.dateFrom.getUTCMonth() + 1, av.dateFrom.getUTCDate());
      let to = new NgbDate(av.dateTo.getUTCFullYear(), av.dateTo.getUTCMonth() + 1, av.dateTo.getUTCDate()+2);

      if((from?.after(this.fromDate) && from.before(date)) || (to.after(this.fromDate) && (to.before(date)))) {
        result = false;
      }
    })

    return result;
  }

  selectFromDate(value: NgbDate) {
    this.reserveFrom.emit(value);
  }

  selectToDate(value: NgbDate) {
    this.reserveTo.emit(value);
  }
}
