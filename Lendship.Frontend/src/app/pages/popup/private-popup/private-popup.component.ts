import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {UntypedFormBuilder, Validators} from "@angular/forms";
import {Conversation, IConversation} from "../../../models/conversation";
import {ConversationService} from "../../../services/conversation/conversation.service";
import {Router} from "@angular/router";
import {UserService} from "../../../services/user/user.service";
import {User} from "../../../models/user";

@Component({
  selector: 'app-private-popup',
  templateUrl: './private-popup.component.html',
  styleUrls: ['./private-popup.component.scss']
})
export class PrivatePopupComponent implements OnInit {
  @Input() addedUsers = new Array<User>();
  @Output() user = new EventEmitter<User>();
  error = "";

  privateUserForm = this.formBuilder.group({
    userEmail: ['', [Validators.required]]
  });

  constructor(
    private formBuilder: UntypedFormBuilder,
    private userService: UserService) {

  }

  ngOnInit(): void {
  }
  checkUser() {
    this.userService.getUserByEmail(this.privateUserForm.value.userEmail)
      .subscribe((user) => {
        if (user != null){
          if (!this.userAlreadyAdded(user)){
            this.user.emit(user);
          }
          else {
            this.error = "User is already added.";
          }
        } else {
          this.error = "User not found with this email address";
        }
      });
  }

  userAlreadyAdded(user: User): Boolean {
    let result = false;
    this.addedUsers.forEach(u => {
      if (u.id === user.id){
        result = true;
      }
    });
    return result;
  }
}
