import {User} from "./user";

export interface Conversation{
  id: number,
  advertisementId: number,
  conversationName: string,
  users: User[]
  userString: string,
  hasNewMessage: boolean
}
