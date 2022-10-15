import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule} from "@angular/forms";
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';
import {AutocompleteLibModule} from 'angular-ng-autocomplete';

import { ZXingScannerModule } from '@zxing/ngx-scanner';

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
import { ConversationsPageComponent } from './pages/conversations-page/conversations-page.component';
import { ConversationInfoPageComponent } from './pages/conversation-info-page/conversation-info-page.component';
import { MessagePopupComponent } from './pages/popup/message-popup/message-popup.component';
import { ReservationPageComponent } from './pages/reservation-page/reservation-page.component';
import { NotificationsPageComponent} from "./pages/notifications-page/notifications-page.component";
import { ErrorPageComponent} from "./pages/error-page/error-page.component";
import {TokenInterceptor} from "./interceptors/token.interceptor";
import {EvaluationPopupComponent} from "./pages/popup/evaluation-popup/evaluation-popup.component";
import {QrcodeComponent} from "./pages/grcode-page/qrcode.component";
import { QRCodeModule } from 'angularx-qrcode';

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
        ReservationPageComponent,
        NotificationsPageComponent,
        ErrorPageComponent,
        EvaluationPopupComponent,
        QrcodeComponent,
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
    AutocompleteLibModule,
    ZXingScannerModule,
    QRCodeModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
