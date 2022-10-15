import {Component, OnInit} from '@angular/core';
import {environment} from "../../../environments/environment";
import {ReservationService} from "../../services/reservation/reservation.service";
import {IReservationBasic} from "../../models/reservation-basic";
import {Router} from "@angular/router";

@Component({
  selector: 'app-qrcode',
  templateUrl: './qrcode.component.html',
  styleUrls: ['./qrcode.component.scss']
})
export class QrcodeComponent implements OnInit {
  baseUrl = environment.baseUrl;
  reservations = new Array<IReservationBasic>();
  token: string | undefined;
  closing = true;
  loading = true;

  scannerEnabled: boolean = false;
  information: string = "No se ha detectado información de";

  error = false;

  constructor(private reservationService: ReservationService,
              private router: Router)
  {
    reservationService.getReservationBasics()
      .subscribe(result => {
        console.log(result);
        this.reservations = result;
        this.loading = false;
      });
  }

  public scanSuccessHandler($event: any) {
    this.scannerEnabled = false;
    this.information = "Siker... ";

    this.reservationService.validateReservationToken($event)
      .subscribe(result => {
        if(result){
          this.router.navigateByUrl('home');
        } else {
          this.error = true;
        }
      });
  }

  public setScanner(value: boolean) {
    this.scannerEnabled = !this.scannerEnabled;
  }

  ngOnInit(): void {
  }

  getReservationToken(reservation: IReservationBasic) {
    this.reservationService.getReservationToken(reservation.reservationId, this.closing)
      .subscribe(result => this.token = result);
  }

  validateReservationToken(token: string) {
    this.reservationService.validateReservationToken(token);
  }

  setClosing(){
    this.closing = !this.closing;
  }
}
