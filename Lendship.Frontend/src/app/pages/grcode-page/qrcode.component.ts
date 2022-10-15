import {Component, OnInit} from '@angular/core';
import {environment} from "../../../environments/environment";
import {ReservationService} from "../../services/reservation/reservation.service";
import {IReservationBasic} from "../../models/reservation-basic";
import {ZXingScannerComponent} from "@zxing/ngx-scanner";
import { Result } from '@zxing/library';

@Component({
  selector: 'app-qrcode',
  templateUrl: './qrcode.component.html',
  styleUrls: ['./qrcode.component.scss']
})
export class QrcodeComponent implements OnInit {
  baseUrl = environment.baseUrl;
  reservations = new Array<IReservationBasic>();
  token = "";
  closing = true;
  loading = true;

  scannerEnabled: boolean = true;
  information: string = "No se ha detectado información de";

  constructor(private reservationService: ReservationService)
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

  }

  public enableScanner() {
    this.scannerEnabled = !this.scannerEnabled;
    this.information = "enable";
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
