import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-delete-maamar',
  templateUrl: './delete-maamar.component.html',
  styleUrls: ['./delete-maamar.component.css']
})
export class DeleteMaamarComponent implements OnInit {
  onClose: Subject<any>;
  contact: any = {};
  constructor(
    public modalRef: BsModalRef
  ) { }

  ngOnInit(): void {
    this.onClose = new Subject();
  }

  Delete(success){

    this.onClose.next({
       success
    });
    this.modalRef.hide()
  }

  cancal(){
    this.onClose.next({
      cancel: true
   });
    this.modalRef.hide()
  }
}
