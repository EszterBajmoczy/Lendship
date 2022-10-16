import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UntypedFormBuilder, Validators} from "@angular/forms";
import {IConversation} from "../../../models/conversation";
import {ConversationService} from "../../../services/conversation/conversation.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-info-popup',
  templateUrl: './info-popup.component.html',
  styleUrls: ['./info-popup.component.scss']
})
export class InfoPopupComponent implements OnInit {
  @Input() information: string | undefined;

  constructor() {
  }

  ngOnInit(): void {
  }
}
