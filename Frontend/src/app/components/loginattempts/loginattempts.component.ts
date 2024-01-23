import {Component, OnDestroy, OnInit} from '@angular/core';
import {LoginAttemptResponse} from "../../types/Login-Attempt-Response";
import {Subscription} from "rxjs";
import {LoginattemptsService} from "../../services/loginattempts.service";
import {DateFormatting} from "../../utils/DateFormatting";

@Component({
  selector: 'app-loginattempts',
  templateUrl: './loginattempts.component.html',
  styleUrls: ['./loginattempts.component.css']
})
export class LoginattemptsComponent implements OnInit, OnDestroy {
  logs: LoginAttemptResponse [] = [];
  subscription?: Subscription;

  constructor(private loginService: LoginattemptsService) {
  }

  ngOnInit(): void {
    this.subscription = this.loginService.getMyLoginAttempts().subscribe({
      next: (response) => {
        this.logs = response;
        for(let i = 0; i < this.logs.length; i++) {
          this.logs[i].time = DateFormatting.formatDateTime(this.logs[i].time);
        }
      },
      error: (error) => console.log(error)
    })

  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
