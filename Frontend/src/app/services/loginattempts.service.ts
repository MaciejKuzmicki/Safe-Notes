import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthService} from "./auth.service";
import {finalize, Observable, tap} from "rxjs";
import {LoginAttemptResponse} from "../types/Login-Attempt-Response";
import {NoteGetModel} from "../types/Note-Get.model";

@Injectable({
  providedIn: 'root'
})
export class LoginattemptsService {

  constructor(private http: HttpClient,private authService: AuthService) { }

  getMyLoginAttempts(): Observable<LoginAttemptResponse []> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.state().value.jwt}`
    }).set('UserId', this.authService.state().value.userId);
    return this.http.get<LoginAttemptResponse []>('http://localhost:8000/LoginAttempts', {headers}).pipe(
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
