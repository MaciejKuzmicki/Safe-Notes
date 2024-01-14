import {Component, OnDestroy, OnInit} from '@angular/core';
import {NoteGetModel} from "../../types/Note-Get.model";
import {Subscription} from "rxjs";
import {NoteService} from "../../services/note.service";

@Component({
  selector: 'app-notelist',
  templateUrl: './notelist.component.html',
  styleUrls: ['./notelist.component.css']
})
export class NotelistComponent implements OnInit, OnDestroy{
  notes: NoteGetModel [] = [];
  subscription?: Subscription;

  constructor(private noteService: NoteService) {
  }
  ngOnInit(): void {
    this.subscription = this.noteService.getNotes().subscribe({
      next: (response) => {this.notes = response},
      error: (error) => console.log(error)
    })
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

}
