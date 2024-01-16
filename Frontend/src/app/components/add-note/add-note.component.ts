import {Component, OnDestroy, OnInit} from '@angular/core';
import {Form, FormBuilder, FormGroup, Validators} from "@angular/forms";
import {NoteCreateModel} from "../../types/Note-Create.model";
import {Subscription} from "rxjs";
import {PasswordStrengthChecker} from "../../utils/PasswordStrengthChecker";
import {NoteService} from "../../services/note.service";
import DOMPurify from 'dompurify';
import {Router} from "@angular/router";


@Component({
  selector: 'app-add-note',
  templateUrl: './add-note.component.html',
  styleUrls: ['./add-note.component.css']
})
export class AddNoteComponent implements OnInit, OnDestroy {
  myForm!: FormGroup;
  myPasswordForm!: FormGroup;
  model: NoteCreateModel = {
    password: '',
    content: '',
    encrypted: false,
    ispublic: false,
    title: '',
  }
  passwordEntropy: number = 0;
  passwordSubscription?: Subscription;
  subscription?: Subscription;
  errorMessage: string = '';
  constructor(private formBuilder: FormBuilder, private noteService: NoteService, private router: Router) {
  }


  onFormSubmit(): void {
    this.model.content = DOMPurify.sanitize(this.myForm.get('content')?.value, {
      ALLOWED_TAGS: ['b', 'i', 'em', 'strong', 'a', 'img', 'h1', 'h2', 'h3', 'h4', 'h5'],
      ALLOWED_ATTR: ['href', 'src', 'alt']
    });
    this.model.title = this.myForm.get('title')?.value;
    if(this.myForm.valid && !this.model.encrypted) {
      this.subscription = this.noteService.addNote(this.model).subscribe({
        next: () => {
          this.router.navigateByUrl('/');
        },
        error: (error) => this.errorMessage = "Something went wrong"
      })
    }
    else if(this.myForm.valid && this.myPasswordForm.valid) {
      this.model.password = this.myPasswordForm.get('password')?.value;
      this.model.ispublic = false;
      this.subscription = this.noteService.addNote(this.model).subscribe({
        next: () => {
          this.router.navigateByUrl('/');

        },
        error: (error) => this.errorMessage = "Something went wrong"
      })
    }
    else this.errorMessage = "Data are incorrect";
  }

  hideError() {
    this.errorMessage = '';
  }


  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      content: ['', [Validators.required, Validators.maxLength(500)]],
      title: ['', [Validators.required, Validators.maxLength(20)]],
    });
    this.myPasswordForm = this.formBuilder.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
    });

    this.passwordSubscription = this.myPasswordForm.get('password')?.valueChanges.subscribe(
      (passwordTyped) => {
        this.passwordEntropy = PasswordStrengthChecker.calculateEntropy(passwordTyped);
      }
    );
  }

  ngOnDestroy(): void {
    this.passwordSubscription?.unsubscribe();
  }
}
