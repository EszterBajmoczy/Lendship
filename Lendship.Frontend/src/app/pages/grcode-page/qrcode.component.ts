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
  operationClose: boolean = false;
  isMessage: boolean = false;

  @ViewChild('evaluationPopUp')
  private evaluationPopUp: TemplateRef<any> | undefined;

  @ViewChild('infoPopup')
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
        this.evaluationBasic = new EvaluationBasic(result.otherUser, result.reservationId, result.advertisementId, result.isLender, result.message)
        this.operationClose = result.operation === "Close";

        if (result.operation === "Close") {
          if (result.message !== undefined && result.message != null && result.message != ""){
            this.isMessage = true;
          }
          this.openEvaluation(this.evaluationPopUp, result.message)
        } else {
          if (result.message !== undefined && result.message != null && result.message != "") {
            this.isMessage = true;
            this.openInformation(this.informationPopup)
          } else{
            console.log("???????!!!!!!!")
            console.log(result)
            this.evaluationBasic.message = "Wrong QR code!"
            this.isMessage = true;
            this.openInformation(this.informationPopup)
          }
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

  openEvaluation(content: any, msg: string | null) {
    console.log(this.evaluationBasic)
    console.log(this.evaluationBasic)
    if (this.evaluationBasic !== undefined){
      this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result
        .catch((res) => {
          console.log("B")
          console.log(this.isMessage)
          if (this.isMessage) {
            this.openInformation(this.informationPopup)
          }
        });
    }
  }

  openInformation(content: any) {
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'})
      .result
        .then((res) => { this.router.navigateByUrl('home') })
        .catch((res) => { this.router.navigateByUrl('home') });
  }

  submitEvaluationAdvertiser(evaluation: EvaluationAdvertiser) {
    this.modalService.dismissAll()
    this.userService.createEvaluationAdvertiser(evaluation)
      .subscribe(result => {
        this.reservationService.admitReservation(evaluation.reservationId)
          .subscribe(res => { });
      });
  }

  submitEvaluationLender(evaluation: EvaluationLender) {
    this.modalService.dismissAll()
    this.userService.createEvaluationLender(evaluation)
      .subscribe(result => {
        this.reservationService.admitReservation(evaluation.reservationId)
          .subscribe(res => { });
      });
  }
}
