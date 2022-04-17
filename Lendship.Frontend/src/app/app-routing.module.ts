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
    path: 'advertisements/new',
    component: AdvertisementCreateComponent
  },
  {
    path: 'conversations',
    component: ConversationsPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
