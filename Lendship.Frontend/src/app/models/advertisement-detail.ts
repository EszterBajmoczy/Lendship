import {IAvailability} from "./availability";
import {User} from "./user";
import {AdvertisementList} from "./advertisementList";
import {Category} from "./category";

export interface AdvertisementDetail extends AdvertisementList {
  user: User,
  isService: boolean,
  description: string,
  instructionManual: string,
  deposit: number,
  isPublic: boolean,
  category: Category,
  availabilities: IAvailability[],
  imageLocations: string[],
  privateUsers: User[],
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
  privateUsers: User[];

  constructor(id: number, title: string, price: number, credit: number, latitude: number, longitude: number, location: string, imageLocation: string, privateUsers: User[]) {
    this.id = id;
    this.title = title;
    this.price = price;
    this.credit = credit;
    this.latitude = latitude;
    this.longitude = longitude;
    this.location = location;
    this.imageLocation = imageLocation;
    this.privateUsers = privateUsers;
  }
}
