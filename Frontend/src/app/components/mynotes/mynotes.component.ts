import {Component, OnDestroy, OnInit} from '@angular/core';
import {NoteGetModel} from "../../types/Note-Get.model";
import {Subscription} from "rxjs";
import {NoteService} from "../../services/note.service";

@Component({
  selector: 'app-mynotes',
  templateUrl: './mynotes.component.html',
  styleUrls: ['./mynotes.component.css']
})
export class MynotesComponent implements OnInit, OnDestroy{
  notes: NoteGetModel [] = [];
  subscription?: Subscription;

  constructor(private noteService: NoteService) {
  }
  ngOnInit(): void {
    this.subscription = this.noteService.getMyNotes().subscribe({
      next: (response) => {this.notes = response},
      error: (error) => console.log(error)
    })
  }

  changeNote(decryptedNote?: NoteGetModel): void {
    if(decryptedNote) {
      const existingNote = this.notes.find(note => note.noteId === decryptedNote.noteId);
      if (existingNote) {
        existingNote.content = decryptedNote.content;
      }
    }
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
