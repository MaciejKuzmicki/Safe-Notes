import {Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Subscription} from "rxjs";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit, OnChanges{
  subscription?: Subscription;
  constructor(private authService: AuthService) {

  }
  isLoggedIn: boolean = false;
  ngOnInit(): void {
    this.subscription = this.authService.getState().subscribe(
      (data) => this.isLoggedIn = data.isLoggedIn
    )
  }

  logout(): void {
    this.authService.logout();
  }

  ngOnChanges(changes: SimpleChanges): void {
  }
}
