import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import { AdvertisementService } from "../../services/advertisement/advertisement.service";
import { AdvertisementDetail } from "../../models/advertisement-detail";
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal } from "@ng-bootstrap/ng-bootstrap";
import { ReservationService } from "../../services/reservation/reservation.service";
import { Reservation } from "../../models/reservation";
import {AuthService} from "../../services/auth/auth.service";
import {Conversation} from "../../models/conversation";
import {ConversationService} from "../../services/conversation/conversation.service";
import {Message} from "../../models/message";

@Component({
  selector: 'app-advertisement-info-page',
  templateUrl: './advertisement-info.component.html',
  styleUrls: ['./advertisement-info.component.scss']
})
export class AdvertisementInfoComponent implements OnInit {
  id: number = -1;
  ad: AdvertisementDetail | undefined;
  isOwnAdvertisement: boolean = false;

  message = "";
  conversationId = -1;

  closeResult = '';
  hoveredDate: NgbDate | null = null;
  reserveFrom: NgbDate | undefined;
  reserveTo: NgbDate | undefined;

  constructor(
    private adService: AdvertisementService,
    private reservationService: ReservationService,
    private authService: AuthService,
    private conService: ConversationService,
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
      } else if (result == "Send"){
        console.log("ss!!");
        this.sendMessage();
      }
    }).catch((res) => {});
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

  delete() {
    this.adService.deleteAdvertisementById(this.id)
      .subscribe((result) => {
        this.router.navigateByUrl('advertisements');
      })
  }

  msg(msg: string) {
    this.message = msg;
  }

  getConversationId(conId: number) {
    this.conversationId = conId;
  }

  sendMessage() {
    if(this.conversationId == -1){
      let conversation = new Conversation(0, this.id, this.ad?.title ?? "", true);
      console.log("con:");
      console.log(conversation);
      this.conService.createConversation(conversation)
        .subscribe((conId) => {
          this.send(conId);
        });
    } else {
      this.send(this.conversationId);
    }
  }

  send(conId: number){
    let msg = new Message(conId, this.message, true)
    this.conService.sendMessage(msg)
      .subscribe((response) => {
        this.router.navigateByUrl('conversations/' + this.id + '/' + conId);
      });
  }
}
