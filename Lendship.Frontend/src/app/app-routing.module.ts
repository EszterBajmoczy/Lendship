import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent } from "./pages/home-page/home-page.component";
import { LoginComponent } from "./pages/login-page/login.component";
import { RegistrationPageComponent } from "./pages/registration-page/registration-page.component";
import { AdvertisementsPageComponent } from "./pages/advertisements-page/advertisements-page.component";
import { AdvertisementInfoComponent } from "./pages/advertisement-info-page/advertisement-info.component";
import { AdvertisementCreateComponent } from "./pages/advertisement-create/advertisement-create.component";
import { ProfilePageComponent } from "./pages/profile-page/profile-page.component";
import { ConversationsPageComponent } from "./pages/conversations-page/conversations-page.component";
import {ConversationInfoPageComponent} from "./pages/conversation-info-page/conversation-info-page.component";
import {ReservationPageComponent} from "./pages/reservation-page/reservation-page.component";
import {NotificationsPageComponent} from "./pages/notifications-page/notifications-page.component";
import {ErrorPageComponent} from "./pages/error-page/error-page.component";
import {QrcodeComponent} from "./pages/grcode-page/qrcode.component";

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'home'
  },
  {
    path: 'home',
    component: HomePageComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'registration',
    component: RegistrationPageComponent
  },
  {
    path: 'advertisements',
    component: AdvertisementsPageComponent
  },
  {
    path: 'advertisement/:id',
    component: AdvertisementInfoComponent
  },
  {
    path: 'profile',
    component: ProfilePageComponent
  },
  {
    path: 'profile/:id',
    component: ProfilePageComponent
  },
  {
    path: 'notifications',
    component: NotificationsPageComponent
  },
  {
    path: 'reservations',
    component: ReservationPageComponent
  },
  {
    path: 'advertisements/new',
    component: AdvertisementCreateComponent
  },
  {
    path: 'advertisements/edit/:advertisementId',
    component: AdvertisementCreateComponent
  },
  {
    path: 'conversations',
    component: ConversationsPageComponent
  },
  {
    path: 'conversations/:advertisementId/:conversationId',
    component: ConversationInfoPageComponent
  },
  {
    path: 'error',
    component: ErrorPageComponent
  },
  {
    path: 'qrcode',
    component: QrcodeComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
