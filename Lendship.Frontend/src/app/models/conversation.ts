import {User} from "./user";

export interface IConversation {
  id: number,
  advertisementId: number,
  conversationName: string,
  users: User[]
  userString: string,
  hasNewMessage: boolean
}

export class Conversation {
  id: number;
  advertisementId: number;
  conversationName: string;
  hasNewMessage: boolean;

  constructor(id: number, advertisementId: number, conversationName: string, hasNewMessage: boolean) {
    this.id = id;
    this.advertisementId = advertisementId;
    this.conversationName = conversationName;
    this.hasNewMessage = hasNewMessage;
  }
}
