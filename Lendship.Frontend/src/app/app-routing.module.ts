import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomePageComponent} from "./pages/home-page/home-page.component";
import { LoginComponent} from "./pages/login-page/login.component";
import { RegistrationPageComponent} from "./pages/registration-page/registration-page.component";
import { AdvertisementsPageComponent} from "./pages/advertisements-page/advertisements-page.component";
import {AdvertisementInfoPageComponent} from "./pages/advertisement-info-page/advertisement-info-page.component";
import { ProfilePageComponent} from "./pages/profile-page/profile-page.component";

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
    component: AdvertisementInfoPageComponent
  },
  {
    path: 'profile',
    component: ProfilePageComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
