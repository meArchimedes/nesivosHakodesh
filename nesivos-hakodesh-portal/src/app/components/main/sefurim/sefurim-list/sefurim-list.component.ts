import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { SefurimDetailsPopupComponent } from '../sefurim-details-popup/sefurim-details-popup.component';

@Component({
  selector: 'app-sefurim-list',
  templateUrl: './sefurim-list.component.html',
  styleUrls: ['./sefurim-list.component.css']
})
export class SefurimListComponent implements OnInit {

  search: any = {
    Page: 1,
    ItemsPerPage: 20,
    sortBy: 'Name',
    sortDirection: 'Ascending',
    
  }
  ListOfSefurim: Array<any> = [];
  totalCount: number = 0;

  @Input() isMergePopup: boolean = false;
  @Output() selectionUpdated = new EventEmitter<any>();

  constructor(private api: ApiService, private util: UtilService, private modalService: BsModalService,) { }

  ngOnInit(): void {
     this.loadSefurim();
  }

  loadSefurim(){
   
    this.util.loadingStart();
    this.ListOfSefurim = [];
    this.api.SefurimList(this.search, success => {
     this.ListOfSefurim = success.list;
   //  console.log('Sefurim list', this.ListOfSefurim)
     this.totalCount = success.totalCount;
     this.util.loadingStop();
    }, error => {
      this.util.loadingStop();
       console.log('error', error)
     //  this.util.showError(error)
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
    this.loadSefurim();

  }

  pageChanged(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      this.loadSefurim();
    }
  }
  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
    this.loadSefurim();
  }

  open(safer) {

    if(this.isMergePopup) {

      this.selectionUpdated.emit(safer);

    } else {

      this.util.navigate('sefurim/' + safer.seferID);
    }
  }

  openCreateModal(delails) {


    var delailsCopy = JSON.parse(JSON.stringify(delails));

      var modalRef = this.modalService.show(SefurimDetailsPopupComponent, { class: "create-modal ngDraggable", backdrop:'static', initialState: {
        contact:{ delails: delailsCopy }
    }});
    modalRef.content.onClose.subscribe(result => {
        this.loadSefurim();
    })
    }

    DeleteSefur(SefurID){
      this.api.DeleteSefur(SefurID, success => {
        this.loadSefurim()
      }, error => {
        // this.util.showError(error)
        this.util.openDeleteSource(error.messages)
      })
    }

    openDeleteSefur(SefurID) {
      var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable", backdrop:'static', initialState: {
        contact:{
          Msg:  '?האם אתה בטוח שהנך רוצה למחוק את ספר לצמיתות',
          Del: true
       }
             } })
  
      modalRef.content.onClose.subscribe(result => {
        if (result.success) {
          this.DeleteSefur(SefurID)
        }
        
    })
    }

}
