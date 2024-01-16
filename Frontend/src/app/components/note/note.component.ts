import {Component, Input, OnInit} from '@angular/core';
import {NoteGetModel} from "../../types/Note-Get.model";



@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent implements OnInit{

  @Input() note: NoteGetModel = {} as NoteGetModel;
  ngOnInit(): void {


  }
}
