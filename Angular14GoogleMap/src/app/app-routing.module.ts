import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MapCircleComponent } from './map-circle/map-circle.component';
import { AuthGuard } from '@auth0/auth0-angular';
import { MapCircle } from '@angular/google-maps';
import { GooglemapComponent } from './googlemap/googlemap.component';

const routes: Routes = [
  {path:'', component: MapCircleComponent, canActivate:[AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
