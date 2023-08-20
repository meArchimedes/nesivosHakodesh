import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-sefurim-details-popup',
  templateUrl: './sefurim-details-popup.component.html',
  styleUrls: ['./sefurim-details-popup.component.css']
})
export class SefurimDetailsPopupComponent implements OnInit {

 NewSefur: any = {}
  contact: any= {};
  onClose: Subject<any>;
  constructor(public modalRef: BsModalRef, private util: UtilService, private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();

     if (this.contact.delails != false) {
     this.NewSefur = this.contact.delails
     }
  }

  Save(){
   // console.log('newSefer', this.NewSefur)

    if (this.NewSefur.seferID) {
     
       this.api.UpdateSefur(this.NewSefur, success => {
        this.modalRef.hide()
        this.onClose.next({
          data: success
        });
       }, error => {
        console.log(error)
        this.util.openDeleteSource(error.messages)
       })
    }
    else{
      this.api.AddSefur(this.NewSefur, success => {
      
      this.modalRef.hide()
        this.onClose.next({
          data: success
        });
      
        }, error => {
          console.log(error)
          this.util.openDeleteSource(error.messages)
    
        })
    }

  }

}
