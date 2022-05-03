import { Component, OnInit } from '@angular/core';
import {IReservation} from "../../models/reservation";
import {ReservationService} from "../../services/reservation/reservation.service";

@Component({
  selector: 'app-reservation-page',
  templateUrl: './reservation-page.component.html',
  styleUrls: ['./reservation-page.component.scss']
})
export class ReservationPageComponent implements OnInit {
  usersReservations = new Array<IReservation>();
  reservationsForUsersAdvertisements = new Array<IReservation>();

  constructor(private reservationService: ReservationService) {
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

}
