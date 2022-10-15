export interface IReservationBasic {
  reservationId: number;
  advertisementName: string;
  isOwn: number;
  dates: string;
}

export class ReservationToken {
  reservationtoken: string;

  constructor(token: string) {
    this.reservationtoken = token;
  }
}
