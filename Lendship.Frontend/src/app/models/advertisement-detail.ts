import {IAvailability} from "./availability";
import {User} from "./user";
import {Advertisement} from "./advertisement";

export interface AdvertisementDetail extends Advertisement {
  user: User,
  description: string,
  instructionManual: string,
  deposit: number,
  isPublic: boolean,
  category: string,
  availabilities: IAvailability[],
  imageLocations: string[],
  creation: string
}

export class AdvertisementDetail implements AdvertisementDetail {
  id: number;
  title: string;
  price: number;
  credit: number;
  latitude: number;
  location: string;
  longitude: number;
  imageLocation: string;

  constructor(id: number, title: string, price: number, credit: number, latitude: number, longitude: number, location: string, imageLocation: string) {
    this.id = id;
    this.title = title;
    this.price = price;
    this.credit = credit;
    this.latitude = latitude;
    this.longitude = longitude;
    this.location = location;
    this.imageLocation = imageLocation;
  }
}
