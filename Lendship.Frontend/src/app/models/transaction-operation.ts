import {User} from "./user";

export interface ITransactionOperation {
  success: boolean;
  operation: string;
  credit: number;
  message: string;
  otherUser: User;
  reservationId: number;
  advertisementId: number;
  isLender: boolean;
}
