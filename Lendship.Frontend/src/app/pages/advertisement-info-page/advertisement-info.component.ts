import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import { AdvertisementService } from "../../services/advertisement/advertisement.service";
import { AdvertisementDetail } from "../../models/advertisement-detail";
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ReservationService } from "../../services/reservation/reservation.service";
import { Reservation } from "../../models/reservation";
import {AuthService} from "../../services/auth/auth.service";

@Component({
  selector: 'app-advertisement-info-page',
  templateUrl: './advertisement-info.component.html',
  styleUrls: ['./advertisement-info.component.scss']
})
export class AdvertisementInfoComponent implements OnInit {
  id: number = -1;
  ad: AdvertisementDetail | undefined;
  isOwnAdvertisement: boolean = false;

  closeResult = '';
  hoveredDate: NgbDate | null = null;
  reserveFrom: NgbDate | undefined;
  reserveTo: NgbDate | undefined;

  constructor(
    private adService: AdvertisementService,
    private reservationService: ReservationService,
    private authService: AuthService,
    private modalService: NgbModal,
    private calendar: NgbCalendar,
    public formatter: NgbDateParserFormatter,
    private router: Router,
    activatedRoute: ActivatedRoute)
  {
    activatedRoute.params.subscribe( params => {
      this.id = params['id'];

      console.log("id " +this.id);
      this.adService.getAdvertisementDetailById(this.id)
        .subscribe(ad => {
          console.log(ad);
          this.ad = ad;

          this.isOwnAdvertisement = authService.getUserId() === ad.user.id;
          console.log("!!");
          console.log(ad.user.id);
          console.log(authService.getUserId());
        });
    });
  }

  ngOnInit(): void {
  }

  open(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then((result) => {
      console.log("result");
      console.log(result);
      if(result == "Reserve"){
        console.log("do reservation");

        if(this.reserveTo !== undefined && this.reserveFrom !== undefined) {
          let res = new Reservation(0, 1, this.reserveFrom, this.reserveTo);

          this.reservationService.reserve(this.id, res)
            .subscribe(response => {
              console.log(response);
            });
        }
        //TODO error msg
      }
    });
  }

  isHovered(date: NgbDate) {
    return this.reserveFrom && !this.reserveTo && this.hoveredDate && date.after(this.reserveFrom) &&
      date.before(this.hoveredDate);
  }


  reserveDateFrom(from: NgbDate) {
    this.reserveFrom = from;
  }

  reserveDateTo(to: NgbDate) {
    this.reserveTo = to;
  }

  edit() {
    this.router.navigateByUrl('advertisements/edit/'+ this.id);
  }
}
