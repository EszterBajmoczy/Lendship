import {Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {environment} from "../../../environments/environment";
import {ReservationService} from "../../services/reservation/reservation.service";
import {IReservationBasic} from "../../models/reservation-basic";
import {Router} from "@angular/router";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {EvaluationAdvertiser, EvaluationLender} from "../../models/evaluation";
import {UserService} from "../../services/user/user.service";
import {EvaluationBasic} from "../../models/evaluation-basic";

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

  evaluationBasic: EvaluationBasic | undefined;

  @ViewChild('evaluationPopUp')
  private evaluationPopUp: TemplateRef<any> | undefined;

  @ViewChild('sendMsgPopup')
  private informationPopup: TemplateRef<any> | undefined;

  constructor(private reservationService: ReservationService,
              private userService: UserService,
              private router: Router,
              private modalService: NgbModal)
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
        //TODO
        // siker -> reserve van elég kredit, vagy nem kell
        // sikertelen -> reserve nincs kredit, Message
        // siker -> close van kredit, vagy nem kell
        // sikertelen -> close nincs elég kredit vagy kevesebb, Message

        if (result.operation === "Close") {
          this.evaluationBasic = new EvaluationBasic(result.otherUser, result.reservationId, result.advertisementId, result.isLender, result.message)
          this.openEvaluation(this.evaluationPopUp)
        } else {
          //TODO info mennyi lett lefoglalva
          this.openInformation(this.evaluationPopUp, result.message)
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
      .subscribe(result => {
        this.token = result.reservationToken;
        this.evaluationBasic = new EvaluationBasic(result.otherUser, result.reservationId, result.advertisementId, result.isLender);
        console.log(result);
      });
  }

  setClosing(){
    this.closing = !this.closing;
  }

  openEvaluation(content: any) {
    console.log("open dialog");

    if(this.evaluationBasic !== undefined){
      var modalRef = this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
      modalRef.result.catch((res) => {  this.router.navigateByUrl('home') });

    }
    //modalRef.componentInstance.userTo = otherUser;
    //modalRef.componentInstance.advertisementId = advertisementId;
    //modalRef.componentInstance.reservationId = reservationId;
    //modalRef.componentInstance.isLender = isLender;
    //modalRef.componentInstance.message = message;

  }

  openInformation(content: any, message: string) {
    console.log("open dialog");

    var modalRef = this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'});
    modalRef.componentInstance.information = message;

    modalRef.result.catch((res) => {  this.router.navigateByUrl('home') });
  }

  submitEvaluationAdvertiser(evaluation: EvaluationAdvertiser) {
    this.modalService.dismissAll()
    this.userService.createEvaluationAdvertiser(evaluation)
      .subscribe(result => {
        this.reservationService.updateReservationsState(evaluation.reservationId, "Closed")
          .subscribe(res => { this.router.navigateByUrl('home');});
      });
  }

  submitEvaluationLender(evaluation: EvaluationLender) {
    this.modalService.dismissAll()
    this.userService.createEvaluationLender(evaluation)
      .subscribe(result => {
        this.reservationService.updateReservationsState(evaluation.reservationId, "Closed")
          .subscribe(res => { this.router.navigateByUrl('home');});
      });
  }
}
