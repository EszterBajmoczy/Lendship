import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AppHeaderComponent} from "./components/common/header/app-header.component";
import { AppFooterComponent} from "./components/common/footer/app-footer.componenet";
import { LoginComponent } from './pages/login-page/login.component';
import { RegistrationPageComponent } from "./pages/registration-page/registration-page.component";
import { AdvertisementsPageComponent } from './pages/advertisements-page/advertisements-page.component';
import { AdvertisementInfoComponent } from './pages/advertisement-info-page/advertisement-info.component';
import { AdvertisementCreateComponent } from './pages/advertisement-create/advertisement-create.component';
import { ProfilePageComponent } from './pages/profile-page/profile-page.component';
import { ReservationPopupComponent } from './pages/popup/reservation-popup/reservation-popup.component';
import { CreateAdvertisementPopupComponent } from "./pages/popup/create-advertisement-popup/create-advertisement-popup.component";
import { FileUploadComponent } from './components/common/file-upload/file-upload/file-upload.component';
import { TokenInterceptor } from "./interceptors/token.interceptor";
import { ConversationsPageComponent } from './pages/conversations-page/conversations-page.component';
import { ConversationInfoPageComponent } from './pages/conversation-info-page/conversation-info-page.component';
import { MessagePopupComponent } from './pages/popup/message-popup/message-popup.component';

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        AppHeaderComponent,
        AppFooterComponent,
        LoginComponent,
        RegistrationPageComponent,
        AdvertisementsPageComponent,
        AdvertisementInfoComponent,
        AdvertisementCreateComponent,
        ProfilePageComponent,
        ReservationPopupComponent,
        CreateAdvertisementPopupComponent,
        FileUploadComponent,
        ConversationsPageComponent,
        ConversationInfoPageComponent,
        MessagePopupComponent,
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
