import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthService} from "./auth.service";
import {finalize, Observable, tap} from "rxjs";
import {LoginAttemptResponse} from "../types/Login-Attempt-Response";
import {NoteGetModel} from "../types/Note-Get.model";
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class LoginattemptsService {

  constructor(private http: HttpClient,private authService: AuthService, private cookie: CookieService) { }

  getMyLoginAttempts(): Observable<LoginAttemptResponse []> {
    const headers = new HttpHeaders({
      'Authorization': this.cookie.get('Authorization')
    }).set('UserId', this.authService.getUser()?.userId as string );
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
