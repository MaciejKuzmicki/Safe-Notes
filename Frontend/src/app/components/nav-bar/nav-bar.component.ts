import {Component, OnChanges, OnInit, SimpleChanges} from '@angular/core';
import {AuthService} from "../../services/auth.service";
import {Subscription} from "rxjs";
import {User} from "../../types/User";
import {Router, RouterLink} from "@angular/router";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit, OnChanges{
  subscription?: Subscription;
  constructor(private authService: AuthService, private router: Router) {

  }
  user?: User;
  ngOnInit(): void {
    this.subscription = this.authService.user().subscribe({
      next: (response) => {
        this.user = response;
      }
    });
    this.user = this.authService.getUser();
  }

  logout(): void {
    this.authService.logout();
    this.router.navigateByUrl('/');
  }

  ngOnChanges(changes: SimpleChanges): void {
  }
}
