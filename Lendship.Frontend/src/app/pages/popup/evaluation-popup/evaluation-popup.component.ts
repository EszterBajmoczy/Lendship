import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder } from "@angular/forms";
import {EvaluationAdvertiser, EvaluationLender} from "../../../models/evaluation";
import {User} from "../../../models/user";

@Component({
  selector: 'app-evaluation-popup',
  templateUrl: './evaluation-popup.component.html',
  styleUrls: ['./evaluation-popup.component.scss']
})
export class EvaluationPopupComponent implements OnInit {
  @Input() userTo: User | undefined = undefined;
  @Input() advertisementId: number = 0;
  @Input() reservationId: number = 0;
  @Input() isLender = false;

  @Output() evaluationAdvertiser = new EventEmitter<EvaluationAdvertiser>();
  @Output() evaluationLender = new EventEmitter<EvaluationLender>();

  flexibility = 0;
  reliability = 0;
  qualityAtReturn = 0;
  qualityOfProduct = 0;

  evaluationForm = this.formBuilder.group({
    comment: [''],
    isAnonymous: [false]
  });

  constructor(private formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
  }

  addFlexibilityStars(stars: number) {
    this.flexibility = stars;
  }

  addReliabilityStars(stars: number) {
    this.reliability = stars;
  }

  addQualityAtReturnStars(stars: number) {
    this.qualityAtReturn = stars;
  }

  addQualityOfProductStars(stars: number) {
    this.qualityOfProduct = stars;
  }

  onSubmit() {
    console.log(this.userTo)
    console.log(this.isLender)
    console.log("!")
    console.log()
    if (this.isLender && this.userTo !== undefined) {
      let lenderEv = new EvaluationLender(this.userTo, this.advertisementId, this.reservationId, this.flexibility, this.reliability, this.qualityAtReturn, this.evaluationForm.value.comment, this.evaluationForm.value.isAnonymous)
      console.log(lenderEv)
      this.evaluationLender.emit(lenderEv);
    } else if (this.userTo !== undefined) {
      let advertiserEv = new EvaluationAdvertiser(this.userTo, this.advertisementId, this.reservationId, this.flexibility, this.reliability, this.qualityOfProduct, this.evaluationForm.value.comment, this.evaluationForm.value.isAnonymous)
      console.log(advertiserEv)
      this.evaluationAdvertiser.emit(advertiserEv);
    }
  }
}
