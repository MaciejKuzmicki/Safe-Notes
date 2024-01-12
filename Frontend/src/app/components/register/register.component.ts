import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, ValidationErrors, Validators} from "@angular/forms";
import {Subscription} from "rxjs";
import {AuthService} from "../../services/auth.service";
import {RegisterRequestModel} from "../../types/Register-Request.model";
import {RegisterResponseModel} from "../../types/Register-Response.model";
import {error} from "@angular/compiler-cli/src/transformers/util";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy{
  myForm!: FormGroup;
  errorMessage: string = '';
  private Subscription?: Subscription;
  model: RegisterRequestModel = {
    Email: '',
    Password: '',
  };
  response: RegisterResponseModel = {
    TOTPSecret: '',
  };

  constructor(private formBuilder: FormBuilder, private authService: AuthService) {

  }

  ngOnDestroy(): void {
    this.Subscription?.unsubscribe();
  }

  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.minLength(1), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      password2: ['', [Validators.required, Validators.minLength(8)]],
    }, {validators: this.passwordMatchValidator});
  }

  onSubmit(): void {
    if(this.myForm.valid) {
      this.model.Email = this.myForm.get('email')?.value;
      this.model.Password = this.myForm.get('password')?.value;
      this.Subscription = this.authService.register(this.model).subscribe({
        next: (response) => {
          this.response = response;
        },
        error: (error) => this.errorMessage = "Something went wrong...",
        },
      )
    }
    else {
      this.errorMessage = "Your data are incorrect";
    }
  }

  passwordMatchValidator(formGroup: FormGroup): ValidationErrors | null {
    if (formGroup.get('password')?.value === formGroup.get('password2')?.value) {
      return null;
    } else {
      return { passwordMismatch: true };
    }
  }

  hideError() {
    this.errorMessage = '';
  }

}
