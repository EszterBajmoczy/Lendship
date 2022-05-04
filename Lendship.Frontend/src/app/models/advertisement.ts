import {User} from "./user";

export interface Advertisement {
  id: number,
  title: string,
  price: number,
  credit: number,
  latitude: number,
  longitude: number,
  location: string,
  imageLocation: string,
  user: User
}
