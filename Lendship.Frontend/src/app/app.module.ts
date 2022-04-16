import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule} from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

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
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
    FormsModule,
    NgbModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
