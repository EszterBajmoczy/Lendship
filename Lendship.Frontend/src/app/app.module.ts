import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { AppHeaderComponent} from "../common/components/header/app-header.component";
import { AppFooterComponent} from "../common/components/footer/app-footer.componenet";

@NgModule({
    declarations: [
        AppComponent,
        HomePageComponent,
        AppHeaderComponent,
        AppFooterComponent
    ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
