import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {NoteCreateModel} from "../types/Note-Create.model";
import {finalize, Observable, tap} from "rxjs";
import {NoteGetModel} from "../types/Note-Get.model";
import {AuthService} from "./auth.service";

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  constructor(private http: HttpClient, private authService: AuthService ) { }

  addNote(model: NoteCreateModel): Observable<void> {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${this.authService.defaultState.jwt}`
    }).set('UserId', this.authService.defaultState.userId);
    console.log(this.authService.defaultState.userId); //empty
    console.log(this.authService.defaultState.jwt); //empty
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

  getNotes(): Observable<NoteGetModel []> {
    return this.http.get<NoteGetModel []>('').pipe(
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
