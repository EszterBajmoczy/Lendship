import {User} from "./user";
import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

export class Message {
  id: number;
  conversationId: number;
  UserFrom: User;
  content: string;
  new: boolean;
  date: Date;
  dateString: string | undefined;
  own: boolean

  constructor(id: number, conversationId: number, userFrom: User, content: string, isNew: boolean, date: Date) {
    this.id = id;
    this.conversationId = conversationId;
    this.UserFrom = userFrom;
    this.content = content;
    this.new = isNew;
    this.date = date;
    this.own = false;
  }
}
