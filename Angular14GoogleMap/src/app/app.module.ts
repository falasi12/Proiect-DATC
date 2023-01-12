import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { GoogleMapsModule } from "@angular/google-maps";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AppNavigationBarComponent } from './app-navigation-bar/app-navigation-bar.component';
import { GooglemapComponent } from './googlemap/googlemap.component';
import { MapCircleComponent } from './map-circle/map-circle.component';
import { ApiService } from './services/api.service';
import { HttpClientModule } from "@angular/common/http";
import { AuthModule } from "@auth0/auth0-angular";
import { AuthButtonComponent } from "./authButton.component";

@NgModule({
  declarations: [
    AppComponent,
    AppNavigationBarComponent,
    GooglemapComponent,
    MapCircleComponent,
    AuthButtonComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    GoogleMapsModule,
    HttpClientModule,
    AuthModule.forRoot({
      domain: 'dev-dhjjk7tu.us.auth0.com',
      clientId: 'SH7OtCwYTOriM404WH3Ec0dz0Sa3Edqy'
    }),
  ],
  providers: [ApiService],
  bootstrap: [AppComponent]
})
export class AppModule { }
