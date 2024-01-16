import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppComponent } from './app.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { NotelistComponent } from './components/notelist/notelist.component';
import { AddNoteComponent } from './components/add-note/add-note.component';
import { NoteComponent } from './components/note/note.component';
import { PasswordStrengthBarComponent } from './components/password-strength-bar/password-strength-bar.component';
import { MynotesComponent } from './components/mynotes/mynotes.component';

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    LoginComponent,
    NavBarComponent,
    NotelistComponent,
    AddNoteComponent,
    NoteComponent,
    PasswordStrengthBarComponent,
    MynotesComponent,
  ],
    imports: [
        BrowserModule,
        NgbModule,
        AppRoutingModule,
        FormsModule,
        HttpClientModule,
        ReactiveFormsModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
