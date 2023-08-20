import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { MergeParagraphPopupComponent } from '../../maamarim/merge-paragraph-popup/merge-paragraph-popup.component';
import { MergeMaamarPopUpComponent } from '../merge-maamar-pop-up/merge-maamar-pop-up.component';

import { TorahsNewPopupComponent } from '../torahs-new-popup/torahs-new-popup.component';

@Component({
  selector: 'app-torahs-list',
  templateUrl: './torahs-list.component.html',
  styleUrls: ['./torahs-list.component.css']
})
export class TorahsListComponent implements OnInit {

  search: any = {
    Page: 1,
    ItemsPerPage: 20,
    Sefurim: [],
    Parsha: [],
    sortBy: 'TorahID',
    sortDirection: 'Ascending',
  }

  SefurimSearch: any = {
    Page: 1,
    ItemsPerPage: 20,
    Sefurim: [],
    Parsha: [],
    sortBy: 'Name',
    sortDirection: 'Ascending',
  }
  TorahList: Array<any> = []
  totalCount: number = 0;

  ListOfSefurim: Array<any> = [];
  obListOfParsha: Array<any> = [];

  @Input() isMergePopup: boolean = false;
  @Output() selectionUpdated = new EventEmitter<any>();
  constructor(  private util: UtilService, private api: ApiService, private modalService: BsModalService,) { }

  ngOnInit(): void {

    //check if we have a saved search object
    var savedSearch = this.util.GetAndDeleteSheardData('torahs-list-saved-search');
   // console.log('savedSearch', savedSearch);

    if(savedSearch.value) {

      this.ListOfSefurim = savedSearch.ListOfSefurim;
      this.obListOfParsha =  savedSearch.obListOfParsha;
      this.search = savedSearch.search;

    } else {

      
      this.loadSefurim();
      this.loadParshas();
    }
    
    this.LoadTorahsList();
  }
 
  LoadTorahsList(){

    this.util.loadingStart();
  
  this.api.TorahsList(this.search, success => {
    
   //.log('LoadTorahsList', success);
    this.util.loadingStop();
    this.TorahList = success.list
    this.totalCount = success.totalCount;

  }, error => {
    this.TorahList = [];
    console.log(error);
    this.util.loadingStop();
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
    this.LoadTorahsList();

  }

  pageChanged(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      this.LoadTorahsList();
    }
  }
  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = e;
    this.LoadTorahsList();
  }

  loadParshas(){

    this.api.getParshas2(success => {
   
      this.obListOfParsha = success

     }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }


  loadSefurim(){
   
    this.api.SefurimList(this.SefurimSearch, success => {
     this.ListOfSefurim = success.list;
    }, error => {
       console.log('error', error)

     //  this.util.showError(error)
     this.util.openDeleteSource(error.messages)
    })
  }

  openTorah(torah)  {

    if(this.isMergePopup) {

      this.openParagraph(torah);

    } else {

      this.util.navigate(`sefurim/${torah.sefer.seferID}?torah=${torah.torahID}`);
    }
  }

  onDropDownOpenChange(e) {

    if(!e) {
      this.checked();
    }

  }

  openMergeModal(torah) {
    var modalRef = this.modalService.show(MergeMaamarPopUpComponent, { class: "merge-modal ngDraggable" ,backdrop:'static', initialState: {
      thora: torah
    }});
    
    modalRef.content.onClose.subscribe(result => {
     // console.log('result Merge Pop Up',result)
      

  })
  }

 
  SelectedParsha() { 
    var text ="";
     for (var name in this.GetAllSelectedParshas()) {
      text +=  ", " + this.GetAllSelectedParshas()[name].name;
    }  return text }

    SelectedSefurim() { 
      var text ="";
       for (var name in this.ListOfSefurim.filter(x => x.selected)) {
        text += ", " + this.ListOfSefurim.filter(x => x.selected)[name].name;
      }  return text }


  SelectedCountParsha() {return this.GetAllSelectedParshas().length;}
  SelectedCountSefurim() {return this.ListOfSefurim.filter(x => x.selected).length;}

  checked(){

    this.search.Parsha = []
    this.search.Sefurim = []
   
    var parsha =  this.GetAllSelectedParshas().forEach(x => {
      this.search.Parsha.push(x.name)
    });
    var Topices=  this.ListOfSefurim.filter(x => x.selected).forEach(x => {
      this.search.Sefurim.push(x.name)
    });

    
    this.LoadTorahsList()
  }


  TorahCheckedUpdates() {

    var selectedMaamarim = this.TorahList.filter(x => x.selected);
    this.selectionUpdated.emit(selectedMaamarim);
  }

  GetAllSelectedParshas() {

    var selected = [];

    this.obListOfParsha.forEach(pb => {
      
      pb.parsesObjects.forEach(x => {
        if(x.selected) {
          selected.push(x);
        }
      });

    });

    return selected;
  }
  isAllParshasChecked(s)  {

    return s.parsesObjects.every(x => x.selected);
  }
  checkParshaBook(s) {

    if(this.isAllParshasChecked(s)) {

      s.parsesObjects.forEach(x => {
        x.selected = false;
      });

    } else {

      s.parsesObjects.forEach(x => {
        x.selected = true;
      });
    }

  }

  openDeleteMaamar(torahId) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable",backdrop:'static',  initialState: {
      contact:{
        Msg:  '?האם אתה בטוח שהנך רוצה למחוק את תורה לצמיתות',
        Del: true
     }
           }})

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {
        this.api.DeleteTorah(torahId, success => {
          this.LoadTorahsList()
        }, error => {
          this.util.openDeleteSource(error.messages)
        })
      }
      
  })
  }

  openCreateModal() {
    var modalRef = this.modalService.show(TorahsNewPopupComponent, { class: "create-modal ngDraggable", backdrop:'static', initialState: {
      contact:{
          Sefurim: this.ListOfSefurim,
          Parshas: this.obListOfParsha,
         
      } 
  }});
  modalRef.content.onClose.subscribe(result => {
      console.log('result',result)
      this.LoadTorahsList();
  })
  }


  openParagraph(torah) {
    var modalRef = this.modalService.show(MergeParagraphPopupComponent, { class: "merge-modal-popup ngDraggable" ,backdrop:'static', initialState: {
      contact: torah.torahParagraphs,
      
    }});
  //   modalRef.content.onClose.subscribe(result => {
     

  // })
  }
}
