import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { pointOfInterestDTO } from '../models/pointOfInterestDTO';


@Injectable({
    providedIn: 'root'
})

export class ApiService{
    private baseApiUrl = `https://apiambrosiaproject.azurewebsites.net/Info`;

    public getPointsOfInterest(){
        return this._http.get(`${this.baseApiUrl}/AllPOI`) as Observable<pointOfInterestDTO[]>;
    }

    public deletePointOfInterest(id: number, radius:number, latitude: number, longitude: number){
        return this._http.get(`${this.baseApiUrl}/deletePointOfInterest?id=${id}&&latitude=${latitude}&&Longitude=${longitude}&&radius=${radius}`,{});

    }

    public checkUser(username: string, password:string){
        return this._http.get(`${this.baseApiUrl}/userLogin?Username=${username}&&Password=${password}`) as Observable<boolean>;
    }

    constructor(private _http: HttpClient){


    }

}