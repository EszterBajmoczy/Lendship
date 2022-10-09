import { Component, OnInit } from '@angular/core';
import {ReservationService} from "../../services/reservation/reservation.service";
import {IReservationDetail} from "../../models/reservation-detail";
import {Router} from "@angular/router";
import {User} from "../../models/user";
import {NgbCalendar, NgbDate, NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {NgbDateHandlerService} from "../../services/date-handler/ngb-date-handler.service";
import {EvaluationAdvertiser, EvaluationLender} from "../../models/evaluation";
import {UserService} from "../../services/user/user.service";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-reservation-page',
  templateUrl: './reservation-page.component.html',
  styleUrls: ['./reservation-page.component.scss']
})
export class ReservationPageComponent implements OnInit {
  baseUrl = environment.baseUrl;
  loadingUsersReservations = true;
  loadingReservationsForUser = true;

  usersReservations = new Array<IReservationDetail>();
  reservationsForUsersAdvertisements = new Array<IReservationDetail>();

  selectedUsersReservations = new Array<IReservationDetail>();
  selectedReservationsForUsersAdvertisements = new Array<IReservationDetail>();
  showMyReservations = true;

  showUsers = true;
  showReservationsForUser = true;

  model: NgbDate;
  date: { year: number; month: number; } | undefined;

  constructor(private reservationService: ReservationService,
              private userService: UserService,
              private ngbDateHandler: NgbDateHandlerService,
              private calendar: NgbCalendar,
              private modalService: NgbModal,
              private router: Router) {
    this.model = calendar.getToday();
    reservationService.getUsersReservations()
      .subscribe(res => {
        console.log(res)
        this.usersReservations = this.initializeNgbDateFields(res);
        this.loadingUsersReservations = false;
      });

    reservationService.getReservationsForUsersAdvertisement()
      .subscribe(res => {
        console.log(res)
        this.reservationsForUsersAdvertisements = this.initializeNgbDateFields(res);
        this.loadingReservationsForUser = false;
      });
  }

  initializeNgbDateFields(res: IReservationDetail[]){
    res.forEach(r => {
      r.dateFromNgbDate = this.ngbDateHandler.convertDateToNgbDate(r.dateFrom, true);
      r.dateToNgbDate = this.ngbDateHandler.convertDateToNgbDate(r.dateTo, false)
    });

    return res;
  }

  ngOnInit(): void {
  }

  onDateSelection(date: NgbDate) {
    this.selectedUsersReservations = new Array<IReservationDetail>();
    this.selectedReservationsForUsersAdvertisements = new Array<IReservationDetail>();

    this.usersReservations.forEach(res => {
      if(date.after(res.dateFromNgbDate) && date.before(res.dateToNgbDate)) {
        this.selectedUsersReservations.push(res);
      }
    });

    this.reservationsForUsersAdvertisements.forEach(res => {
      if(date.after(res.dateFromNgbDate) && date.before(res.dateToNgbDate)) {
        this.selectedReservationsForUsersAdvertisements.push(res);
      }
    });

    this.showMyReservations = this.selectedUsersReservations.length != 0;
  }

  accept(resId: number) {
    this.reservationService.updateReservationsState(resId, "Accepted")
      .subscribe(res => {
        this.usersReservations.forEach(res => {
          if(res.id == resId){
            res.reservationState= "Accepted";
          }
        });
      });
  }

  decline(resId: number) {
    this.reservationService.updateReservationsState(resId, "Declined")
      .subscribe(res => {
        this.usersReservations.forEach(res => {
          if(res.id == resId){
            res.reservationState= "Declined";
          }
        });
      });
  }

  close(resId: number) {
    this.reservationService.admitReservation(resId)
      .subscribe(res => {
        this.usersReservations.forEach(res => {
          if(res.id == resId){
            res.reservationState= "Closed";
            return;
          }
        });
        this.reservationsForUsersAdvertisements.forEach(res => {
          if(res.id == resId){
            res.reservationState= "Closed";
            return;
          }
        });
      });
  }

  resign(resId: number) {
    this.reservationService.updateReservationsState(resId, "Resigned")
      .subscribe(res => {
        this.usersReservations.forEach(res => {
          if(res.id == resId){
            res.reservationState= "Resigned";
          }
        });
      });
  }

  reservationClicked(res: IReservationDetail) {
    this.router.navigate(['advertisement', res.advertisement.id]);
  }

  userClicked(user: User) {
    //TODO navigate to profil page
    this.router.navigate(['home']);
  }

  reservedByUser(date: NgbDate){
    let result = false;
    this.usersReservations?.forEach( res => {
      if(date.after(res.dateFromNgbDate) && date.before(res.dateToNgbDate)) {
        result = true;
      }
    });
    return result;
  }

  reservedToUser(date: NgbDate){
    let result = false;
    this.reservationsForUsersAdvertisements?.forEach( res => {

      if(date.after(res.dateFromNgbDate) && date.before(res.dateToNgbDate)) {
        result = true;
      }
    });
    return result;
  }

  notInThisMonth(date: NgbDate){
    let result = false;
    if(date.month != this.date?.month){
      return true;
    }
    return result;
  }

  submitEvaluationAdvertiser(evaluation: EvaluationAdvertiser) {
    this.modalService.dismissAll()
    this.userService.createEvaluationAdvertiser(evaluation)
      .subscribe(result => {
        this.reservationService.updateReservationsState(evaluation.reservationId, "Closed")
          .subscribe(res => {
            this.usersReservations.forEach(res => {
              if(res.id == evaluation.reservationId){
                res.reservationState= "Closed";
                return;
              }
            });
          });
      });
  }

  submitEvaluationLender(evaluation: EvaluationLender) {
    this.modalService.dismissAll()
    this.userService.createEvaluationLender(evaluation)
      .subscribe(result => {
        this.reservationService.updateReservationsState(evaluation.reservationId, "Closed")
          .subscribe(res => {
            this.reservationsForUsersAdvertisements.forEach(res => {
              if(res.id == evaluation.reservationId){
                res.reservationState= "Closed";
                return;
              }
            });
          });
      });
  }

  open(content: any, res: IReservationDetail) {
    console.log("open dialog");
    console.log(res);

    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.catch((res) => {});
  }
}
