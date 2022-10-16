import {User} from "./user";

export interface IReservationToken {
  reservationToken: string;
  otherUser: User;
  reservationId: number;
  advertisementId: number;
  isLender: boolean;
}

export class QRToken {
  reservationToken: string;

  constructor(token: string) {
    this.reservationToken = token;
  }
}
