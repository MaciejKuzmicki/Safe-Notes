import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {NoteCreateModel} from "../types/Note-Create.model";
import {finalize, Observable, tap} from "rxjs";
import {NoteGetModel} from "../types/Note-Get.model";

@Injectable({
  providedIn: 'root'
})
export class NoteService {

  constructor(private http: HttpClient) { }

  addNote(model: NoteCreateModel): Observable<void> {
    return this.http.post<void>('', model).pipe(
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
