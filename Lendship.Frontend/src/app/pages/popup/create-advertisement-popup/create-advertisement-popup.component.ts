import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { NgbCalendar, NgbDate } from "@ng-bootstrap/ng-bootstrap";
import { ReservationService} from "../../../services/reservation/reservation.service";
import {IAvailability} from "../../../models/availability";

@Component({
  selector: 'app-create-popup',
  templateUrl: './create-advertisement-popup.component.html',
  styleUrls: ['./create-advertisement-popup.component.scss']
})
export class CreateAdvertisementPopupComponent implements OnInit {
  reserved: IAvailability[] | undefined;

  hoveredDate: NgbDate | null = null;

  fromDate: NgbDate | null = null;
  toDate: NgbDate | null = null;

  @Output() reserveFrom = new EventEmitter<NgbDate>();
  @Output() reserveTo = new EventEmitter<NgbDate>();

  constructor(private calendar: NgbCalendar, private reservationService: ReservationService) {
  }

  ngOnInit(): void {
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      this.reserveFrom.emit(date);
    } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
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

  selectFromDate(value: NgbDate) {
    this.reserveFrom.emit(value);
  }

  selectToDate(value: NgbDate) {
    this.reserveTo.emit(value);
  }
}
