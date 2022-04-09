import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";
import { GeocodingService} from "../services/geocoding/geocoding.service";
import {Injectable} from "@angular/core";
import {mapEntry} from "@angular/compiler/src/output/map_util";
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
            console.log(response);
            console.log(response['error']);
            if(response['error'] != null){
              return { locationValidator: {value: control.value}};
            }
            return null;
          }
        )
      );
  }
}
