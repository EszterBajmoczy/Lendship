import {Availability} from "./availability";
import {User} from "./user";

export interface AdvertisementDetail {
  id: number,
  user: User,
  title: string,
  description: string,
  InstructionManual: string,
  price: number,
  credit: number,
  deposit: number,
  latitude: number,
  longitude: number,
  location: string
  isPublic: boolean,
  category: string,
  availabilities: Availability[],
  imageLocations: string[],
  creation: string
}
