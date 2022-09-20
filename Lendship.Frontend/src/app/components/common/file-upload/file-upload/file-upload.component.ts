import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { FileUploadService } from "../../../../services/file-upload/file-upload.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.scss']
})
export class FileUploadComponent implements OnInit {
  @Input() advertisementId: number = 0;
  @Output() files = new EventEmitter<File>();

  loading: boolean = false;

  constructor(private fileUploadService: FileUploadService, private router: Router) { }

  ngOnInit(): void {
  }

  addNewItem(file: File) {
    this.files.emit(file);
  }

  onSelected(event: Event) {
    if(event != null && event.target != null){
      let targetFiles = (<HTMLInputElement>event.target).files
      if(targetFiles != null){
        for (let i = 0; i < targetFiles.length; i++) {
          this.addNewItem(targetFiles[i]);
        }
      }
    }
  }
}
