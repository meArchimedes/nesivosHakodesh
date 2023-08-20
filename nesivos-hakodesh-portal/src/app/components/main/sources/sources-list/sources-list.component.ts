import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { error } from 'protractor';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { SourcesDetailsPopupComponent } from '../sources-details-popup/sources-details-popup.component';

@Component({
  selector: 'app-sources-list',
  templateUrl: './sources-list.component.html',
  styleUrls: ['./sources-list.component.css']
})
export class SourcesListComponent implements OnInit {

  search: any = {
    Page: 1,
    ItemsPerPage: 20,
    Type :[],
    Topic: [],
    Parsha: [],
    Source: [],
    sortBy: 'FirstName',
    sortDirection: 'Ascending',
  }
  
  ListOfSources: Array<any> = [];
  totalCount: number = 0;

  constructor(private api: ApiService, private util: UtilService, private modalService: BsModalService,) { }

  ngOnInit(): void {
   
    this.loadSource();
  }
 
  
  loadSource(){
   
    this.util.loadingStart();
    this.ListOfSources = [];
    this.api.SourceList(this.search, success => {
     this.ListOfSources = success.list;
     this.totalCount = success.totalCount;
   //  console.log('source', this.ListOfSources)
     this.util.loadingStop();
    }, error => {
       console.log('error', error)
       this.util.loadingStop();
      // this.util.showError(error)
      this.util.openDeleteSource(error.messages)
    })
  }

  getSortClass(prop) {

    if(prop == this.search.sortBy) {

      return (this.search.sortDirection == 'Ascending' ? 'fas fa-sort-down' : 'fas fa-sort-up')
    }

    return 'fas fa-sort';
  }

  sort(prop) {

    if(prop == this.search.sortBy) {

      this.search.sortDirection = (this.search.sortDirection == 'Ascending' ? 'Descending' : 'Ascending')
    } else {

      this.search.sortBy = prop;
      this.search.sortDirection = 'Ascending';

    }
    this.search.Page = 1;
    this.loadSource();

  }

  pageChanged(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      this.loadSource();
    }
  }
  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
    this.loadSource();
  }

  openCreateModal(delails) {

    var delailsCopy = JSON.parse(JSON.stringify(delails));

    var modalRef = this.modalService.show(SourcesDetailsPopupComponent, { class: "create-modal ngDraggable", backdrop:'static', initialState: {
      contact:{

        delails: delailsCopy
        }
  }});
  modalRef.content.onClose.subscribe(result => {
      //console.log('result',result)
      this.loadSource();
  })
  }

  DeleteSource(SourceID){
    this.api.DeleteSource(SourceID, success => {
      this.loadSource();
    }, error => {
      // this.util.showError(error)
      this.util.openDeleteSource(error.messages)
    })
  }



  openDeleteSource(SourceID,) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "create-modal ngDraggable", backdrop:'static', initialState: {
      contact:{
         Msg: '?האם אתה בטוח שהנך רוצה למחוק את מקור לצמיתות',
         Del: true
      }
      
           } })

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {
        this.DeleteSource(SourceID)
      }
      
  })
  }




 
}
