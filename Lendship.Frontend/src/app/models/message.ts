import {User} from "./user";
import {NgbDate} from "@ng-bootstrap/ng-bootstrap";

export class Message {
  id: number | undefined;
  conversationId: number;
  UserFrom: User | undefined;
  content: string;
  new: boolean;
  date: Date | undefined;
  dateString: string | undefined;
  own: boolean

  constructor(conversationId: number, content: string, isNew: boolean) {
    this.conversationId = conversationId;
    this.content = content;
    this.new = isNew;
    this.own = false;
  }
}
