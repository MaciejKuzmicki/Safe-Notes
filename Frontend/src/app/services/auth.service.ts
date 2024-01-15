import { Injectable } from '@angular/core';
import {finalize, Observable, tap} from "rxjs";
import {RegisterResponseModel} from "../types/Register-Response.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {RegisterRequestModel} from "../types/Register-Request.model";
import {LoginRequestModel} from "../types/Login-Request.model";
import {LoginResponseModel} from "../types/Login-Response.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  register(model: RegisterRequestModel): Observable<RegisterResponseModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<RegisterResponseModel>('https://localhost:44313/Auth/register', model,httpOptions).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }

  login(model: LoginRequestModel): Observable<LoginResponseModel> {
    return this.http.post<LoginResponseModel>('http://localhost:48294/Auth/login', model).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }
}
