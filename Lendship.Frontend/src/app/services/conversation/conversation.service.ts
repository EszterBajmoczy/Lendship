import { Injectable } from '@angular/core';
import {AuthService} from "../auth/auth.service";
import {environment} from "../../../environments/environment";
import {HttpClient, HttpErrorResponse, HttpHeaders} from "@angular/common/http";
import {map, Observable, throwError} from "rxjs";
import {Conversation, IConversation} from "../../models/conversation";
import {catchError} from "rxjs/operators";
import {AdvertisementDetail} from "../../models/advertisement-detail";
import {Message} from "../../models/message";

@Injectable({
  providedIn: 'root'
})
export class ConversationService {
  private baseUrl: string;
  private headers: HttpHeaders;

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
        }),
        catchError(this.handleError));
  }

  getMessagesForConversation(conversationId: number): Observable<Message[]> {
    return this.http.get<Message[]>(this.baseUrl + conversationId, { headers: this.headers})
      .pipe(
        catchError(this.handleError));
  }

  sendMessage(msg: Message) {
    return this.http.post<Message>(this.baseUrl + "msg", msg, { headers: this.headers })
      .pipe(
        catchError(this.handleError));
  }

  createConversation(con: Conversation): Observable<number> {
    return this.http.post<number>(this.baseUrl, con, { headers: this.headers })
      .pipe(
        catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong.
      console.error(
        `Backend returned code ${error.status}, body was: `, error.error);
    }
    // Return an observable with a user-facing error message.
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
