import { Component, OnInit } from '@angular/core';
import {Conversation} from "../../models/conversation";
import {ConversationService} from "../../services/conversation/conversation.service";

@Component({
  selector: 'app-conversation-page',
  templateUrl: './conversation-page.component.html',
  styleUrls: ['./conversation-page.component.scss']
})
export class ConversationPageComponent implements OnInit {
  conversations = new Array<Conversation>();

  constructor(private conversationService: ConversationService) {
    conversationService.getAllConversation()
      .subscribe((cons) => {
        console.log(cons);
        this.conversations = cons;
      });
  }

  ngOnInit(): void {
  }

}
