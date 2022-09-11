import { Component, OnInit } from '@angular/core';
import {IConversation} from "../../models/conversation";
import {ConversationService} from "../../services/conversation/conversation.service";
import {Router} from "@angular/router";

@Component({
  selector: 'app-conversations-page',
  templateUrl: './conversations-page.component.html',
  styleUrls: ['./conversations-page.component.scss']
})
export class ConversationsPageComponent implements OnInit {
  conversations = new Array<IConversation>();
  loading = true;

  constructor(private conversationService: ConversationService, private router: Router) {
    conversationService.getAllConversation()
      .subscribe((cons) => {
        console.log(cons);
        this.conversations = cons;
        this.loading = false;
      });
  }

  ngOnInit(): void {
  }

  openConversation(advertisementId: number, conversationId: number) {
    this.router.navigateByUrl('conversations/' + advertisementId + "/" + conversationId);
  }
}
