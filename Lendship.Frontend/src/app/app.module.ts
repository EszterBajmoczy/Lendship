import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule} from "@angular/forms";
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AppHeaderComponent} from "./components/common/header/app-header.component";
import { AppFooterComponent} from "./components/common/footer/app-footer.componenet";
import { LoginComponent } from './pages/login-page/login.component';
import { RegistrationPageComponent } from "./pages/registration-page/registration-page.component";
import { AdvertisementsPageComponent } from './pages/advertisements-page/advertisements-page.component';
import { AdvertisementInfoPageComponent } from './pages/advertisement-info-page/advertisement-info-page.component';
import { AdvertisementCreateComponent } from './pages/advertisement-create/advertisement-create.component';


@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        AppHeaderComponent,
        AppFooterComponent,
        LoginComponent,
        RegistrationPageComponent,
        AdvertisementsPageComponent,
        AdvertisementInfoPageComponent,
        AdvertisementCreateComponent,
    ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpClientModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
