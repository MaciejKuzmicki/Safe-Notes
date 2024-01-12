import { Injectable } from '@angular/core';
import {finalize, Observable, tap} from "rxjs";
import {RegisterResponseModel} from "../types/Register-Response.model";
import {HttpClient} from "@angular/common/http";
import {RegisterRequestModel} from "../types/Register-Request.model";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  register(model: RegisterRequestModel): Observable<RegisterResponseModel> {
    return this.http.post<RegisterResponseModel>('', model).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {},
      )
    );
  }
}
