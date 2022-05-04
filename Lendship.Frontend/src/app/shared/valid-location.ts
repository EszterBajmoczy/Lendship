import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { GeocodingService} from "../services/geocoding/geocoding.service";
import {Injectable} from "@angular/core";
import {map} from "rxjs";

@Injectable()
export class LocationValidator {
  constructor(private geoCoder: GeocodingService) {

  }

  exists(control: AbstractControl) {
    return this.geoCoder.getLatLong(control.value)
      .pipe(
        map(
          response => {
            if(response['status'] != "OK"){
              return { locationValidator: {value: control.value}};
            }
            return null;
          }
        )
      );
  }
}
