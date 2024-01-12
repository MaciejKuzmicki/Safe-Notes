import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginRequestModel} from "../../types/Login-Request.model";
import {Subscription} from "rxjs";
import {AuthService} from "../../services/auth.service";
import {LoginResponseModel} from "../../types/Login-Response.model";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit{
  myForm!: FormGroup;
  model: LoginRequestModel = {
    Email: '',
    Password: '',
    TotpCode: '',
  }
  subscription?: Subscription;
  response: LoginResponseModel = {
    Token: '',
  }
  errorMessage: string = '';

  constructor(private formBuilder: FormBuilder, private authService: AuthService) {

  }
  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.minLength(1), Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      totp: ['', [Validators.required, Validators.min(100000), Validators.max(999999)]],
    });
  }

  onSubmit(): void {
    if(this.myForm.valid) {
      this.model.Email = this.myForm.get('email')?.value;
      this.model.Password = this.myForm.get('password')?.value;
      this.model.TotpCode = this.myForm.get('totp')?.value;
      this.subscription = this.authService.login(this.model).subscribe({
        next: (response) => {
          this.response = response;
        },
        error: (error) => this.errorMessage = "Something went wrong",
        },
      )
    }
    else this.errorMessage = "Your data are incorrect";
  }

}
