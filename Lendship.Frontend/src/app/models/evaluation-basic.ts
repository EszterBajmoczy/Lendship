import {User} from "./user";

export class EvaluationBasic {
  userTo: User;
  reservationId: number;
  advertisementId: number;
  isLender: boolean;
  message: string;

  constructor(userTo: User, reservationId: number, advertisementId: number, isLender: boolean, message: string = "") {
    this.userTo = userTo;
    this.reservationId = reservationId;
    this.advertisementId = advertisementId;
    this.isLender = isLender;
    this.message = message;
  }
}
