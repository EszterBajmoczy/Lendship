import {User} from "./user";

export interface EvaluationAdvertiser {
  userFrom: User,
  advertisementId: string,
  flexibility: number,
  reliability: number,
  qualityOfProduct: number,
  comment: string,
  creation: string
}

export interface EvaluationLender {
  userFrom: User,
  advertisementId: string,
  flexibility: number,
  reliability: number,
  qualityOfProduct: number,
  comment: string,
  creation: string
}
