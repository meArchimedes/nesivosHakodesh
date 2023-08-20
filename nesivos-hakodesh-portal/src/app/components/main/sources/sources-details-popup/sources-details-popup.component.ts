import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-sources-details-popup',
  templateUrl: './sources-details-popup.component.html',
  styleUrls: ['./sources-details-popup.component.css']
})
export class SourcesDetailsPopupComponent implements OnInit {

  NewSource: any = {}
  contact: any= {};
  onClose: Subject<any>;
  constructor(public modalRef: BsModalRef, public util: UtilService, private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
   
   if (this.contact.delails != false) {
    this.NewSource = this.contact.delails
   }
  }

  click(tab4){
    setTimeout(() => {
      tab4.focus();
      console.log('here')
    },200)
  }
  Save(){
    

    if (this.NewSource.sourceID) {
     
       this.api.UpdateSource(this.NewSource, success => {
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
      this.api.AddSource(this.NewSource, success => {
      
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
