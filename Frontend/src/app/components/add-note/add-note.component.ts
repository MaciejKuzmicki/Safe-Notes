import {Component, OnDestroy, OnInit} from '@angular/core';
import {Form, FormBuilder, FormGroup} from "@angular/forms";
import {NoteCreateModel} from "../../types/Note-Create.model";
import {Subscription} from "rxjs";
import {PasswordStrengthChecker} from "../../utils/PasswordStrengthChecker";

@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit, OnDestroy {
  myForm!: FormGroup;
  model: NoteCreateModel = {
    password: '',
    content: '',
    encrypted: false,
  }
  passwordEntropy: number = 0;
  passwordSubscription?: Subscription;
  constructor(private formBuilder: FormBuilder) {
  }



  ngOnInit(): void {

  }

  ngOnDestroy(): void {
  }
}
