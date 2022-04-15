import {User} from "./user";

export interface UserDetail extends User{
  email: string,
  emailNotification: boolean,
  latitude: number,
  longitude: number,
  location: string,
  registration: string,
}
