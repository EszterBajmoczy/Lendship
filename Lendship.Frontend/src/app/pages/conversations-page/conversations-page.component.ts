import { Component, OnInit } from '@angular/core';
import {IConversation} from "../../models/conversation";
import {ConversationService} from "../../services/conversation/conversation.service";
import {Router} from "@angular/router";
import {environment} from "../../../environments/environment";

@Component({
  selector: 'app-conversations-page',
  templateUrl: './conversations-page.component.html',
  styleUrls: ['./conversations-page.component.scss']
})
export class ConversationsPageComponent implements OnInit {
  baseUrl = environment.baseUrl;
  conversations = new Array<IConversation>();
  loading = true;

  constructor(private conversationService: ConversationService, private router: Router) {
    conversationService.getAllConversation()
      .subscribe((cons) => {
        cons.map(con => {
          if (con.users[0].image === undefined || con.users[0].image.length === 0){
            con.users[0].image = environment.baseImage;
          }
        })
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
