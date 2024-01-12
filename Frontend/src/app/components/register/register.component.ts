import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Subscription} from "rxjs";
import {AuthService} from "../../services/auth.service";
import {RegisterRequestModel} from "../../types/Register-Request.model";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  myForm!: FormGroup;
  errorMessage: string = '';
  private Subscription?: Subscription;
  model: RegisterRequestModel = {
    Email: '',
    Password: '',
  };

  constructor(private formBuilder: FormBuilder, private authService: AuthService) {

  }

  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.minLength(1), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      password2: ['', [Validators.required, Validators.minLength(8)]],
    });
  }

  onSubmit(): void {
    if(this.myForm.valid) {
      this.errorMessage = "Great";
      this.model.Email = this.myForm.value.
      this.Subscription = this.authService.register().subscribe(

      );
    }
    else this.errorMessage = "Not great";
  }

}
