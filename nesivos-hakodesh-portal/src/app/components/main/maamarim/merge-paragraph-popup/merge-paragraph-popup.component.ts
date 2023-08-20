import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-merge-paragraph-popup',
  templateUrl: './merge-paragraph-popup.component.html',
  styleUrls: ['./merge-paragraph-popup.component.css']
})
export class MergeParagraphPopupComponent implements OnInit {
  onClose: Subject<any>;
  contact: any = {};
  constructor(public modalRef: BsModalRef,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
  }

}
