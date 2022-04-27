import {Component, Input, OnInit} from '@angular/core';
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

  private conversations = Array<IConversation>();

  sendMsgForm = this.formBuilder.group({
    id: [],
    conversationId: [],
    content: ['', [Validators.required]],
    new: [true]
  });

  constructor(private formBuilder: FormBuilder, private conService: ConversationService, private router: Router) {
    conService.getAllConversation()
      .subscribe((cons) => this.conversations = cons);
  }

  ngOnInit(): void {
  }


  set conId(value: number) {
    this.sendMsgForm.get("conversationId")?.setValue(value);
  }

  set id(value: number) {
    this.sendMsgForm.get("id")?.setValue(value);
  }

  onSubmit(){
    if(this.sendMsgForm.invalid){
      return;
    }

    let conversationId = this.checkExistingConversations();

    if(conversationId == -1){
      let conversation = new Conversation(0, this.advertisementId, this.advertisementName ?? "", true);
      console.log(conversation);
      this.conService.createConversation(conversation)
        .subscribe((conId) => {
          console.log("!!!!!!!!!!!!!!");
          console.log(conId);
          this.sendMessage(conId);
        });
    } else {
      this.sendMessage(conversationId);
    }
  }

  sendMessage(conId: number){
    this.conId = conId;
    this.id = 0;

    console.log(this.sendMsgForm.value)
    this.conService.sendMessage(this.sendMsgForm.value)
      .subscribe((response) => {
        this.router.navigateByUrl('home');
      });
  }

  checkExistingConversations() {
    let id = -1;
    this.conversations.forEach((con) => {
      if(con.advertisementId == this.advertisementId) {
        id = con.id;
      }
    });
    return id;
  }
}
