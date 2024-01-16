import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {NoteGetModel} from "../../types/Note-Get.model";
import {Form, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Subscription} from "rxjs";
import {NoteService} from "../../services/note.service";
import {NoteEncryptModel} from "../../types/Note-Encrypt.model";



@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent implements OnInit{

  @Input() note: NoteGetModel = {} as NoteGetModel;
  @Output() contentAdded: EventEmitter<NoteGetModel> = new EventEmitter<NoteGetModel>();

  myPasswordForm!: FormGroup;
  subscription?: Subscription;
  errorMessage: string = '';
  model: NoteEncryptModel = {
    password: '',
  }
  constructor(private formBuilder: FormBuilder, private noteService: NoteService) {
  }
  ngOnInit(): void {
    this.myPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
    });
  }

  onFormSubmit(): void {
    if(this.myPasswordForm.valid) {
      this.model.password = this.myPasswordForm.get('password')?.value;
      this.subscription = this.noteService.getNote(this.model, this.note.noteId).subscribe({
        next: (response) => {
          this.contentAdded.emit(response);
          this.note.encrypted= false;
          this.errorMessage = '';
        },
        error: (error) => this.errorMessage = "Something went wrong"
      })
    }
  }
}
