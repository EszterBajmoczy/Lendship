import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {Conversation, IConversation} from "../../../models/conversation";
import {ConversationService} from "../../../services/conversation/conversation.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-message-popup',
  templateUrl: './message-popup.component.html',
  styleUrls: ['./message-popup.component.scss']
})
export class MessagePopupComponent implements OnInit {
  @Input() advertisementId: number = 0;
  @Input() advertisementName: string | undefined;

  @Output() msg = new EventEmitter<string>();
  @Output() conversationId = new EventEmitter<number>();

  sendMsgForm = this.formBuilder.group({
    msgContent: ['', [Validators.required]]
  });

  constructor(private formBuilder: FormBuilder, private conService: ConversationService, private router: Router) {
    conService.getAllConversation()
      .subscribe((cons) => {
        let conId = this.checkForExistingConversations(cons)
        this.conversationId.emit(conId);
      });
  }

  ngOnInit(): void {
  }

  editMsg(){
    this.msg.emit(this.sendMsgForm.value.msgContent);
  }

  checkForExistingConversations(cons: IConversation[]): number {
    let id = -1;
    cons.forEach((con) => {
      if(con.advertisementId == this.advertisementId) {
        id = con.id;
      }
    });
    return id;
  }
}
