import { Component, OnInit } from '@angular/core';
import {ReservationService} from "../../services/reservation/reservation.service";
import {IReservationDetail} from "../../models/reservation-detail";
import {Router} from "@angular/router";
import {User} from "../../models/user";

@Component({
  selector: 'app-reservation-page',
  templateUrl: './reservation-page.component.html',
  styleUrls: ['./reservation-page.component.scss']
})
export class ReservationPageComponent implements OnInit {
  usersReservations = new Array<IReservationDetail>();
  reservationsForUsersAdvertisements = new Array<IReservationDetail>();

  showUsers = true;
  showReservationsForUser = true;

  constructor(private reservationService: ReservationService, private router: Router) {
    reservationService.getUsersReservations()
      .subscribe(res => {
        console.log(res);
        this.usersReservations = res
      });

    reservationService.getReservationsForUsersAdvertisement()
      .subscribe(res => {
        console.log(res);
        this.reservationsForUsersAdvertisements = res
      });
  }

  ngOnInit(): void {
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
}
