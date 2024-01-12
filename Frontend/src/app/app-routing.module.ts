import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {RegisterComponent} from "./components/register/register.component";
import {LoginComponent} from "./components/login/login.component";

const routes: Routes = [
  {
    path:'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  }
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forRoot(routes)
  ],
  exports: [RouterModule]
})
export class AppRoutingModule { }
