import {LoginUser} from "./login-user";

export interface RegisterUser extends LoginUser {
  name: string;
  longitude: string;
  latitude: string;
  location: string;
}
