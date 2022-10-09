import {User} from "./user";

export interface IEvaluationAdvertiser {
  userFrom: User | undefined,
  userTo: User,
  advertisementId: number,
  flexibility: number,
  reliability: number,
  qualityOfProduct: number,
  comment: string,
  isAnonymous: boolean
}

export class EvaluationAdvertiser implements IEvaluationAdvertiser {
  userFrom: User | undefined = undefined
  userTo: User
  advertisementId: number
  reservationId: number
  flexibility: number
  reliability: number
  qualityOfProduct: number
  comment: string
  isAnonymous: boolean

  constructor(userTo: User, advertisementId: number, reservationId: number, flexibility: number, reliability: number, qualityOfProduct: number, comment: string, isAnonymous: boolean) {
    this.userTo = userTo;
    this.advertisementId = advertisementId;
    this.reservationId = reservationId;
    this.flexibility = flexibility;
    this.reliability = reliability;
    this.qualityOfProduct = qualityOfProduct;
    this.comment = comment;
    this.isAnonymous =isAnonymous;
  }
}

export interface IEvaluationLender {
  userFrom: User | undefined,
  userTo: User,
  advertisementId: number,
  flexibility: number,
  reliability: number,
  qualityAtReturn: number,
  comment: string,
  isAnonymous: boolean
}

export class EvaluationLender implements IEvaluationLender {
  userFrom: User | undefined = undefined
  userTo: User
  advertisementId: number
  reservationId: number
  flexibility: number
  reliability: number
  qualityAtReturn: number
  comment: string
  isAnonymous: boolean

  constructor(userTo: User, advertisementId: number, reservationId: number, flexibility: number, reliability: number, qualityAtReturn: number, comment: string, isAnonymous: boolean) {
    this.userTo = userTo;
    this.advertisementId = advertisementId;
    this.reservationId = reservationId
    this.flexibility = flexibility;
    this.reliability = reliability;
    this.qualityAtReturn = qualityAtReturn;
    this.comment = comment;
    this.isAnonymous =isAnonymous;
  }
}
