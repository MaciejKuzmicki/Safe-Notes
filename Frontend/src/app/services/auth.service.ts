import {Injectable, signal} from '@angular/core';
import {BehaviorSubject, finalize, Observable, tap} from "rxjs";
import {RegisterResponseModel} from "../types/Register-Response.model";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {RegisterRequestModel} from "../types/Register-Request.model";
import {LoginRequestModel} from "../types/Login-Request.model";
import {LoginResponseModel} from "../types/Login-Response.model";
import {User} from "../types/User";
import {CookieService} from "ngx-cookie-service";



@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private cookie: CookieService) { }

  register(model: RegisterRequestModel): Observable<RegisterResponseModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<RegisterResponseModel>('http://localhost:8000/Auth/register', model,httpOptions).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }

  logout(): void {
    localStorage.removeItem('email');
    localStorage.removeItem('userId');
    this.cookie.delete('Authorization', '/');
    this.$user.next(undefined);
  }
  $user = new BehaviorSubject<User | undefined>(undefined);
  setUser(user: User) {
    this.$user.next(user);
    localStorage.setItem('email', user.email);
    localStorage.setItem('userId', user.userId);
  }

  getUser(): User | undefined {
    const email = localStorage.getItem('email');
    const userId = localStorage.getItem('userId');
    if(email && userId) {
      const user: User = {
        email: email,
        userId: userId
      };
      return user;
    }
    return undefined;
  }

  user(): Observable<User | undefined> {
    return this.$user.asObservable();
  }

  login(model: LoginRequestModel): Observable<LoginResponseModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<LoginResponseModel>('http://localhost:8000/Auth/login', model, httpOptions).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {

        },
        (error) => {console.log(error)},
      )
    );
  }
}


