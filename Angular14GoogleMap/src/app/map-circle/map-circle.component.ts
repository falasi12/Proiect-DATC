import { Component, OnInit } from "@angular/core";
import { pointOfInterestDTO } from "../models/pointOfInterestDTO";
import { googlePOIDTO } from "../models/googlePOIDTO";
import { ApiService } from "../services/api.service";

@Component({
  selector: 'app-map-circle',
  templateUrl: './map-circle.component.html',
  styleUrls: ['./map-circle.component.css']
})
export class MapCircleComponent implements OnInit {

  pointsOfInterest: googlePOIDTO[] = [];

  pointsOfInterestInit: pointOfInterestDTO[]= [];

  constructor( private apiService: ApiService) { }

  ngOnInit(): void {

    this.apiService.getPointsOfInterest().subscribe(result => {
      
      this.pointsOfInterestInit = result;
      this.setUpMap();

    });
   
  }
  center: google.maps.LatLngLiteral = {lat: 24, lng: 12};
  zoom = 4;

  circleCenter: google.maps.LatLngLiteral = {lat: 10, lng: 15};
  radius = 500000000000;

  deletePoint(i: number){
    this.apiService.deletePointOfInterest(this.pointsOfInterestInit[i].id,this.pointsOfInterestInit[i].radius,this.pointsOfInterestInit[i].latitude,this.pointsOfInterestInit[i].longitude).subscribe(result => {

      if(result == true){

        this.pointsOfInterestInit.splice(i, 1);

      }

    })
  }

  goTo(lati: number, long: number){
    this.center = {lat: lati, lng: long};

  }

  setUpMap(){
    this.pointsOfInterestInit.forEach((element) => {
      var center: google.maps.LatLngLiteral = {lat: 24, lng: 12};
      var x: googlePOIDTO = {id: 0, radius:0, circleCenter: center};
      x.id = element.id;
      center = {lat: element.latitude, lng: element.longitude};
      x.circleCenter = center;
      x.radius = element.radius;
      
      this.pointsOfInterest.push(x);
    })
  }
}
