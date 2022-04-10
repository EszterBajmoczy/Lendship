import {Availability} from "./availability";
import {User} from "./user";
import {Advertisement} from "./advertisement";

export interface AdvertisementDetail extends Advertisement {
  user: User,
  description: string,
  instructionManual: string,
  deposit: number,
  isPublic: boolean,
  category: string,
  availabilities: Availability[],
  imageLocations: string[],
  creation: string
}
