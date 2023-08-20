import { Component, NgZone, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { Observable, Subject } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-libray-search',
  templateUrl: './libray-search.component.html',
  styleUrls: ['./libray-search.component.css']
})
export class LibraySearchComponent implements OnInit {

  searchCriteria: any = {
    libraryCategory:  [],
    libraryType: [], 
    LibrarySection:  [],
    LibraryChepter:  [],
  };
  LibraryResult : string;
  LibraryObject: any = {};
  onClose: Subject<any>;
  ListOfCategorys: any =[];
  ListOfTypes: any = [];
  ListOfSections: any = [];
  ListOfChepters: any = [];
  open : boolean = true;
  contactsDataSource: Observable<any>;
  constructor(private api: ApiService, public modalRef: BsModalRef,private zone: NgZone) { }

  ngOnInit() {
    this.onClose = new Subject();


    this.contactsDataSource = Observable.create((observer: any) => {
      observer.next(this.LibraryResult);
    })
    .pipe(mergeMap((token: string) => this.api.SearchLibrary( this.searchCriteria = {
      SearchTerm: token,
      Page: 1,
      itemsPerPage: 10,
      libraryCategory: this.searchCriteria.libraryCategory,
      libraryType:this.searchCriteria.libraryType,
      LibrarySection:this.searchCriteria.LibrarySection,
      LibraryChepter: this.searchCriteria.LibraryChepter,

    })));

    this.loadCategoryFilters();
  }


  selectContact(e: TypeaheadMatch): void {
  //  console.log('selectContact', e);
  // this.masterContact.father = e.item;
    this.LibraryResult = e.item.libraryId;
    this.onClose.next({
      result:  this.LibraryResult,
    });
    this.modalRef.hide()
  }

  contactChanged() {
  //  console.log('contactChanged', this.LibraryResult);
   
    //console.log(this.masterContact.father);
  }

  loadCategoryFilters(){

    this.api.GetCategoryFilters(success => {
      this.ListOfCategorys = success;
      //console.log('success Category', this.ListOfCategorys);
      this.zone.run(() => {});


    }, error => {
       console.log(error)
      //this.util.showError(error.messages);
     })
  }

 

  loadTypeFilters(){
   // console.log('search Types', this.searchCriteria)
    this.api.GetTypeFilters(this.searchCriteria.libraryCategory, success => {
      this.ListOfTypes = success;
    //  console.log('Success Types', this.ListOfTypes);
      this.zone.run(() => {});
    }, error => {
      console.log(error)
    })
  }
  loadSectionFilters(){
 //   console.log('search Section', this.searchCriteria)

    this.api.GetSectionFilters(this.searchCriteria, success => {
      this.ListOfSections = success;
    //  console.log('Success Section', this.ListOfSections);
      this.zone.run(() => {});
    }, error => {
      console.log(error)
    })
  }

  loadChepterFilters(){
  //  console.log('search Chepter', this.searchCriteria)
    this.api.GetChepterFilters(this.searchCriteria, success => {
      this.ListOfChepters = success;
    //  console.log('Success Chepter',  this.ListOfChepters);
      this.zone.run(() => {});
    }, error => {
      console.log(error)
    })
  }

  test(tab1){
  console.log('test')
  tab1.focus();

  this.zone.run(() => {});
 }

 
}
