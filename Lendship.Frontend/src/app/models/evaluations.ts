import {User} from "./user";

export interface EvaluationAdvertiser {
  userFrom: User,
  advertisementId: string,
  flexibility: number,
  reliability: number,
  qualityOfProduct: number,
  comment: string,
  creation: Date,
  creationFormatted: string,
  isAnonymous: boolean
}

export interface EvaluationLender {
  userFrom: User,
  advertisementId: string,
  flexibility: number,
  reliability: number,
  qualityAtReturn: number,
  comment: string,
  creation: Date,
  creationFormatted: string,
  isAnonymous: boolean
}
