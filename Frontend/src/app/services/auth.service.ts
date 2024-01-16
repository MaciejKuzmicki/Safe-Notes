import {Injectable, signal} from '@angular/core';
import {BehaviorSubject, finalize, Observable, tap} from "rxjs";
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
  defaultState: AuthState = {
    isLoggedIn: false,
    email: '',
    userId: '',
    jwt: ''
  }
  logout(): void {
    this.updateState(this.defaultState);
  }
  private stateSubject: BehaviorSubject<AuthState> = new BehaviorSubject(this.defaultState);

  getState(): Observable<AuthState> {
    return this.stateSubject.asObservable();
  }
  state = signal(this.stateSubject);
  private updateState(newState: Partial<AuthState>): void {
    const currentState = this.stateSubject.value;
    const updatedState = { ...currentState, ...newState };
    this.stateSubject.next(updatedState);
  }
  login(model: LoginRequestModel): Observable<LoginResponseModel> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json'
      })
    };
    return this.http.post<LoginResponseModel>('https://localhost:44313/Auth/login', model, httpOptions).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {
          const tokenParts = data.token.split('.');
          const encodedPayload = tokenParts[1];
          const decodedPayload = atob(encodedPayload.replace(/-/g, '+').replace(/_/g, '/'));
          const payload = JSON.parse(decodedPayload);
          this.updateState({
            jwt: data.token,
            email: payload.email,
            isLoggedIn: true,
            userId: payload.nameid
          });
        },
        (error) => {console.log(error)},
      )
    );
  }
}

type AuthState = {
  isLoggedIn: boolean;
  email: string;
  userId: string;
  jwt: string;
}
