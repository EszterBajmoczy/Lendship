import {LoginUser} from "./login-user";

export class RegisterUser extends LoginUser {
  name: string | undefined;
  longitude: string | undefined;
  latitude: string | undefined;
}
