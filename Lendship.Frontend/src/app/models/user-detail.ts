import {User} from "./user";

export interface UserDetail extends User{
  email: string,
  emailNotification: boolean,
  latitude: number,
  longitude: number,
  location: string,
  registration: string,
  image: string,
  credit: number,
  evaluation: EvaluationComputed | undefined
}

export interface EvaluationComputed {
  advertiserFlexibility: number,
  advertiserReliability: number,
  advertiserQualityOfProduct: number,
  lenderFlexibility: number,
  lenderReliability: number,
  lenderQualityAtReturn: number
}
