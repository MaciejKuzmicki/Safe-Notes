import {Component, OnDestroy, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {LoginRequestModel} from "../../types/Login-Request.model";
import {Subscription} from "rxjs";
import {AuthService} from "../../services/auth.service";
import {LoginResponseModel} from "../../types/Login-Response.model";
import {Route, Router} from "@angular/router";
import {CookieService} from "ngx-cookie-service";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy{
  myForm!: FormGroup;
  model: LoginRequestModel = {
    Email: '',
    Password: '',
    TotpCode: '',
  }
  subscription?: Subscription;
  response: LoginResponseModel = {
    token: '',
  }
  errorMessage: string = '';

  constructor(private formBuilder: FormBuilder, private authService: AuthService, private router: Router, private cookie: CookieService) {

  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
  ngOnInit(): void {
    this.myForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.minLength(1), Validators.email, Validators.maxLength(30)]],
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(20)]],
      totp: ['', [Validators.required, Validators.min(100000), Validators.max(999999)]],
    });
  }

  onSubmit(): void {
    this.errorMessage = '';
    if(this.myForm.valid) {
      if(this.retrieveItem('numberOfLoginAttempts') == '3') {
        this.errorMessage = "Try again in a minute";
        return;
      }
      this.model.Email = this.myForm.get('email')?.value;
      this.model.Password = this.myForm.get('password')?.value;
      this.model.TotpCode = this.myForm.get('totp')?.value;
      this.subscription = this.authService.login(this.model).subscribe({
        next: (response) => {
          const tokenParts = response.token.split('.');
          const encodedPayload = tokenParts[1];
          const decodedPayload = atob(encodedPayload.replace(/-/g, '+').replace(/_/g, '/'));
          const payload = JSON.parse(decodedPayload);
          this.response = response;
          this.authService.setUser({
            email: payload.email,
            userId: payload.nameid,
          });
          this.cookie.set('Authorization', `Bearer ${response.token}`, undefined, '/', undefined, true, 'Strict');
          this.router.navigateByUrl('/');
        },
        error: (error) => {
          const value = this.retrieveItem('numberOfLoginAttempts');
          if(value == null) {
            this.storeItem('numberOfLoginAttempts', '1');
          }
          else if(value == '1' ) this.updateItem('numberOfLoginAttempts', '2');
          else if(value == '2') this.updateItem('numberOfLoginAttempts', '3');

            this.errorMessage = "Unauthorized", console.log(error)
        }
      },)
    }
    else this.errorMessage = "Your data are incorrect";
  }

  storeItem(key: string, value: string) {
    const item = {
      value: value,
      expiry: new Date().getTime() + 60000
    };
    localStorage.setItem(key, JSON.stringify(item));
  }

  retrieveItem(key: string) {
    const itemStr = localStorage.getItem(key);
    if (!itemStr) {
      return null;
    }
    const item = JSON.parse(itemStr);
    const now = new Date().getTime();
    if (now > item.expiry) {
      localStorage.removeItem(key);
      return null;
    }
    return item.value.toString();
  }

  updateItem(key: string, newValue: string) {
    const itemStr = localStorage.getItem(key);
    if (itemStr) {
      const item = JSON.parse(itemStr);
      item.value = newValue;
      item.expiry = new Date().getTime() + 60000;
      localStorage.setItem(key, JSON.stringify(item));
    }
  }

}
