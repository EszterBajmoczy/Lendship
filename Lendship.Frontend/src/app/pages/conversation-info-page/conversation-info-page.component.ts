import { Component, OnInit } from '@angular/core';
import {Message} from "../../models/message";
import {ConversationService} from "../../services/conversation/conversation.service";
import {AdvertisementService} from "../../services/advertisement/advertisement.service";
import {ActivatedRoute, Router} from "@angular/router";
import {AdvertisementDetail} from "../../models/advertisement-detail";
import {AuthService} from "../../services/auth/auth.service";
import { DatePipe } from '@angular/common'
import {FormBuilder, Validators} from "@angular/forms";

@Component({
  selector: 'app-conversation-info-page',
  templateUrl: './conversation-info-page.component.html',
  styleUrls: ['./conversation-info-page.component.scss'],
  providers: [DatePipe]
})
export class ConversationInfoPageComponent implements OnInit {
  userId: string;
  loadingMessages = true;

  conversationId: number = 0;
  advertisementId: number = 0;
  advertisement: AdvertisementDetail | undefined;
  adImgLocation: string | undefined;
  messages = new Array<Message>();


  sendMsgForm = this.formBuilder.group({
    id: [],
    conversationId: [],
    content: ['', [Validators.required]],
    new: [true]
  });

  constructor(
    private conService: ConversationService,
    private adService: AdvertisementService,
    private authService: AuthService,
    private datePipe: DatePipe,
    private formBuilder: FormBuilder,
    private router: Router,
    activatedRoute: ActivatedRoute)
  {
    this.userId = authService.getUserId();

    activatedRoute.params.subscribe( params => {
      this.conversationId = params['conversationId'];
      this.advertisementId = params['advertisementId'];
      console.log("conId: " + this.conversationId);
      console.log("adId: " + this.advertisementId);

      this.conService.getMessagesForConversation(this.conversationId)
        .subscribe((msgs) => {
          this.messages = this.categorizeMessages(msgs);
          this.loadingMessages = false;

          this.conService.setMessagesSeen(this.conversationId)
            .subscribe((response) =>{
              console.log(response)
            });
        })

      this.adService.getAdvertisementDetailById(this.advertisementId)
        .subscribe((ad) => {
          this.advertisement = ad;
          this.adImgLocation = "https://localhost:44377/images" + ad.imageLocations.pop();
        })
    });
  }

  ngOnInit(): void {
  }

  private categorizeMessages(msgs: Message[]): Message[] {
    msgs.forEach((msg) => {
      let date = this.dateToString(msg.date!);
      if(date){
        msg.dateString = date;
      }

      if(msg.UserFrom!.id == this.userId){
        msg.own = true;
      } else {
        msg.own = false;
      }

    })

    return msgs;
  }

  dateToString(date: Date): string | null{
    return this.datePipe.transform(date, 'yyyy-MM-dd HH:mm') ?? date.toString();
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
    this.conId = this.conversationId;
    this.id = 0;

    console.log(this.sendMsgForm.value);
    this.conService.sendMessage(this.sendMsgForm.value)
      .subscribe((response) => {
        this.conService.getMessagesForConversation(this.conversationId)
          .subscribe((msgs) => {
            this.messages = this.categorizeMessages(msgs);
          })
      });
  }

  advertisementClicked() {
    this.router.navigateByUrl('advertisement/'+ this.advertisementId);
  }
}
