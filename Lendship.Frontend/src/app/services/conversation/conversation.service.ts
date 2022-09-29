import { Injectable } from '@angular/core';
import {AuthService} from "../auth/auth.service";
import {environment} from "../../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {map, Observable } from "rxjs";
import {Conversation, IConversation} from "../../models/conversation";
import {Message} from "../../models/message";

@Injectable({
  providedIn: 'root'
})
export class ConversationService {
  private readonly baseUrl: string;
  private readonly headers: HttpHeaders;

  constructor(private http: HttpClient, private authService: AuthService) {
    this.baseUrl = environment.baseUrl + "Conversation/";
    this.headers = authService.getHeaders();
  }

  getAllConversation(): Observable<IConversation[]>{
    return this.http.get<IConversation[]>(this.baseUrl, { headers: this.headers})
      .pipe(
        map((cons: IConversation[]) => {
          cons.forEach((con) => {
            let result = "";
            con.users.forEach((user) => {
              if(result == "") {
                result = user.name;
              } else {
                result = result + ", " + user.name;
              }
            })
            con.userString = result;
          });
          return cons;
        }));
  }

  getMessagesForConversation(conversationId: number): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl + conversationId, { headers: this.headers});
  }

  sendMessage(msg: Message) {
    return this.http.post<Message>(this.baseUrl + "msg", msg, { headers: this.headers });
  }

  createConversation(con: Conversation): Observable<number> {
    return this.http.post<number>(this.baseUrl, con, { headers: this.headers });
  }

  setMessagesSeen(conversationId: number): Observable<any> {
    return this.http.post<number>(this.baseUrl + "msg/" + conversationId, { headers: this.headers });
  }
}
