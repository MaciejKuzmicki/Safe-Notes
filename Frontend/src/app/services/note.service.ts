import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {NoteCreateModel} from "../types/Note-Create.model";
import {finalize, Observable, Subscription, tap} from "rxjs";
import {NoteGetModel} from "../types/Note-Get.model";
import {AuthService} from "./auth.service";
import {NoteEncryptModel} from "../types/Note-Encrypt.model";
import {CookieService} from "ngx-cookie-service";

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  subscription?: Subscription;

  constructor(private http: HttpClient, private authService: AuthService, private cookie: CookieService ) { }

  addNote(model: NoteCreateModel): Observable<void> {
    const headers = new HttpHeaders({
      'Authorization': this.cookie.get('Authorization')
    }).set('UserId', localStorage.getItem('userId') as string );
    return this.http.post<void>('http://localhost:8000/Note', model, {headers}).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }

  getMyNotes(): Observable<NoteGetModel []> {
    const headers = new HttpHeaders({
      'Authorization': this.cookie.get('Authorization')
    }).set('UserId', localStorage.getItem('userId') as string );
    return this.http.get<NoteGetModel []>('http://localhost:8000/Note/mynotes', {headers}).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }

  getNotes(): Observable<NoteGetModel []> {
    return this.http.get<NoteGetModel []>('http://localhost:8000/Note').pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {},
        (error) => {console.log(error)},
      )
    );
  }

  getNote(model: NoteEncryptModel, noteId: string): Observable<NoteGetModel> {
    const headers = new HttpHeaders({
      'Authorization': this.cookie.get('Authorization')
    }).set('UserId', localStorage.getItem('userId') as string );
    return this.http.post<NoteGetModel>(`http://localhost:8000/Note/${noteId}`, model, {headers}).pipe(
      finalize(
        () => {}
      ),
      tap(
        (data) => {console.log(data)},
        (error) => {console.log(error)},
      )
    );
  }
}
