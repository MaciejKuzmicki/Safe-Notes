import {Component, Input} from '@angular/core';
import {NoteGetModel} from "../../types/Note-Get.model";

@Component({
  selector: 'app-note',
  templateUrl: './note.component.html',
  styleUrls: ['./note.component.css']
})
export class NoteComponent {
  @Input() note: NoteGetModel = {} as NoteGetModel;
}
