import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule, Routes} from "@angular/router";
import {RegisterComponent} from "./components/register/register.component";
import {LoginComponent} from "./components/login/login.component";
import {NotelistComponent} from "./components/notelist/notelist.component";
import {AddNoteComponent} from "./components/add-note/add-note.component";
import {MynotesComponent} from "./components/mynotes/mynotes.component";

const routes: Routes = [
  {
    path:'',
    component: NotelistComponent
  },
  {
    path:'register',
    component: RegisterComponent
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'addnote',
    component: AddNoteComponent
  },
  {
    path: 'mynotes',
    component: MynotesComponent
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
