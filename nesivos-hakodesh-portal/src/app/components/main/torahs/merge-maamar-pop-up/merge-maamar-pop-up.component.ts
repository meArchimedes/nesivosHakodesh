import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-merge-maamar-pop-up',
  templateUrl: './merge-maamar-pop-up.component.html',
  styleUrls: ['./merge-maamar-pop-up.component.css']
})
export class MergeMaamarPopUpComponent implements OnInit {

  onClose: Subject<any>;

  thora: any = {};
  selectedContacts: Array<any> = [];

  constructor( public modalRef: BsModalRef, private util: UtilService, private api: ApiService) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    //console.log('thora', this.thora)
    
  }

  selectionUpdated(e) {
    this.selectedContacts = e;
  }

  connect() {

    if(this.selectedContacts.length > 0) {

      var newLinks  = [];
      this.thora.sefer.torahs = [];
     // console.log('this.thora',this.thora)
      this.selectedContacts.forEach(element => {
        
        newLinks.push({
          maamar: element,
          torah: this.thora
        });
      });


      this.api.AddMaamarTorahLink(newLinks, success => {

        this.onClose.next({
          success
        });
        this.modalRef.hide()
      }, error => {
        console.log('error', error)
        this.util.openDeleteSource(error.messages)
      });

    
      

    } else {

      this.util.openDeleteSource("אנא בחר מאמר");

    }

  }

}
