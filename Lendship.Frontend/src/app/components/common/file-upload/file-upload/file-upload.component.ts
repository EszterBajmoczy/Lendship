import {Component, Input, OnInit} from '@angular/core';
import { FileUploadService } from "../../../../services/file-upload/file-upload.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {
  @Input() advertisementId: number = 0;
  files = new Array<File>();

  loading: boolean = false;

  constructor(private fileUploadService: FileUploadService, private router: Router) { }

  ngOnInit(): void {
  }

  onSelected(event: Event) {
    if(event != null && event.target != null){
      let targetFiles = (<HTMLInputElement>event.target).files
      if(targetFiles != null){
        for (let i = 0; i < targetFiles.length; i++) {
          this.files.push(targetFiles[i]);
        }
      }
    }
  }

  onUpload() {
    this.loading = !this.loading;
    if(this.files != null){
      this.fileUploadService.upload(this.advertisementId, this.files).subscribe(
        (event: any) => {
          //TODO
          this.router.navigateByUrl('home');
        }
      );
    }
  }
}
