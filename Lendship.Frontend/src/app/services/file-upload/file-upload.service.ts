import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable } from 'rxjs';
import {AuthService} from "../auth/auth.service";
import {environment} from "../../../environments/environment";

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {
  private readonly baseUrl: string;
  private readonly headers: HttpHeaders;

  constructor(private http:HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Image/";
    this.headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.getAccessToken()}`,
    });
  }

  upload(advertisementId: number, files: File[]):Observable<any> {
    const formData = new FormData();
    files.forEach((file) => {
      formData.append(file.name, file);
    })
    console.log(this.baseUrl + advertisementId);
    return this.http.post(this.baseUrl + advertisementId, formData, {headers: this.headers});
  }

  deleteFiles(advertisementId: number, fileNames: string[]){
    fileNames.forEach(fileName => {
      console.log("!!!!!");
      console.log(this.baseUrl + advertisementId + "/" + this.simplifyImageName(fileName));
      return this.http.delete(this.baseUrl + advertisementId + "/" + this.simplifyImageName(fileName), {headers: this.headers})
        .subscribe(s => {console.log("sub");})
    })
  }

  deleteFile(advertisementId: number, fileName: string){
    return this.http.delete(this.baseUrl + advertisementId + "/" + this.simplifyImageName(fileName), {headers: this.headers})
      .subscribe(s => { console.log( fileName + " file is deleted"); })
  }

  private simplifyImageName(name: string): string {
    let i = name.lastIndexOf("\\");
    console.log(name.substring(i+1));
    return name.substring(i + 1);
  }
}
