import { Component, OnInit } from '@angular/core';
import {Conversation} from "../../models/conversation";
import {ConversationService} from "../../services/conversation/conversation.service";

@Component({
  selector: 'app-conversations-page',
  templateUrl: './conversations-page.component.html',
  styleUrls: ['./conversations-page.component.scss']
})
export class ConversationsPageComponent implements OnInit {
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
