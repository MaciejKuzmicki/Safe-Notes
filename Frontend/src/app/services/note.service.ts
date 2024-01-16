import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {NoteCreateModel} from "../types/Note-Create.model";
import {finalize, Observable, Subscription, tap} from "rxjs";
import {NoteGetModel} from "../types/Note-Get.model";
import {AuthService} from "./auth.service";

@Injectable({
  providedIn: 'root'
})
export class NoteService {
  subscription?: Subscription;

  constructor(private http: HttpClient, private authService: AuthService ) { }

  addNote(model: NoteCreateModel): Observable<void> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.state().value.jwt}`
    }).set('UserId', this.authService.state().value.userId);
    return this.http.post<void>('https://localhost:44313/Note', model, {headers}).pipe(
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
      'Authorization': `Bearer ${this.authService.state().value.jwt}`
    }).set('UserId', this.authService.state().value.userId);
    return this.http.get<NoteGetModel []>('https://localhost:44313/Note/mynotes', {headers}).pipe(
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
    return this.http.get<NoteGetModel []>('https://localhost:44313/Note').pipe(
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
