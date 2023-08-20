import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-merge-popup',
  templateUrl: './merge-popup.component.html',
  styleUrls: ['./merge-popup.component.css']
})
export class MergePopupComponent implements OnInit {
  search: any = {
    Page: 1,
    ItemsPerPage: 2000
  }
  TorahList: any = []
  onClose: Subject<any>;
  Maamar: any = {};
  selectedContacts: Array<any> = [];
  constructor(
    public modalRef: BsModalRef, private util: UtilService, private api: ApiService, private modalService: BsModalService,
  ) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    //this.LoadTorahsList();
  }

  
  selectionUpdated(e) {
    this.selectedContacts = e;
  }

  // LoadTorahsList(){
  //   this.api.TorahsList(this.search, success => {
      
  //     this.TorahList = success.list
     
      
  //   }, error => {
  //       this.util.openDeleteSource(error.messages)
  //   })
  //   }

  connect() {

    if(this.selectedContacts.length > 0) {

      var newLinks  = [];

      this.selectedContacts.forEach(element => {
        
        newLinks.push({
          maamar: this.Maamar,
          torah: element
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


    // getSelectedContacts() {
  
    //   //return this.TorahList.filter(x => x.selected);

    //   var selectedTorahs = this.TorahList.filter(x => x.selected)

    //   var newLinks  = [];

    //  selectedTorahs.forEach(element => {
        
    //     newLinks.push({
    //       maamar: this.Maamar,
    //       torah: element,
    //     });
    //   });

   
  
    //      this.api.AddMaamarTorahLink(newLinks, success => {
    //      this.onClose.next({
    //       success
    //    });
    //    this.modalRef.hide()
    //      }, error => {
    //       console.log('error', error)
    //      })
      

  
    // }
    
    

}
