import { Injectable, Output, EventEmitter } from '@angular/core';
import jwt_decode from 'jwt-decode';
import { LocalStorageService} from "../localstorage/localstorage.service";

@Injectable({
  providedIn: 'root',
})
export class JWTTokenService {
  jwtToken: string | undefined;
  decodedToken: { [key: string]: string; } | undefined;

  constructor(private localstorageService: LocalStorageService) {
    var token = localstorageService.get("ACCESS_TOKEN");
    if(token != null && token != ""){
      console.log("T: " +token);
      this.setToken(token);
    }
  }

  setToken(token: string) {
    if (token != null && token != "") {
      this.jwtToken = token;
      this.decodedToken = jwt_decode(this.jwtToken);
    }
  }

  removeToken() {
    this.jwtToken = "";
  }

  decodeToken() {
    if (this.jwtToken != null && this.jwtToken != "") {
      this.decodedToken = jwt_decode(this.jwtToken);
    }
  }

  getDecodeToken() {
    return jwt_decode(<string>this.jwtToken);
  }

  getUserName() {
    if (this.jwtToken != null && this.jwtToken != "") {
      return this.decodedToken ? this.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] : null;
    }
    return "";
  }

  getUserId() {
    if (this.jwtToken != null && this.jwtToken != "") {
      return this.decodedToken ? this.decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'] : null;
    }
    return "";
  }

  getExpiryTime() {
    if (this.jwtToken != null && this.jwtToken != "") {
      return this.decodedToken ? this.decodedToken['exp'] : null;
    }
    return "";
  }

  isTokenExpired(): boolean {
    const expiryTime = parseInt(this.getExpiryTime() ?? '0');
    if (expiryTime) {
      return ((1000 * expiryTime) - (new Date()).getTime()) < 5000;
    } else {
      return false;
    }
  }
}
