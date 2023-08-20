import { Component, OnInit, ViewChild } from '@angular/core';
import { NgSelectComponent } from '@ng-select/ng-select';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead';
import { Observable, Subject } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../delete-maamar/delete-maamar.component';

@Component({
  selector: 'app-new-maamar-popup',
  templateUrl: './new-maamar-popup.component.html',
  styleUrls: ['./new-maamar-popup.component.css']
})
export class NewMaamarPopupComponent implements OnInit {
  onClose: Subject<any>;
  contact: any= {};

  NewMaamar: any = {
    subTopics: [],
    LiberyTitleId: {}
  };
  AddSafer: any = {};
  AddSource : any= {};

  searchCriteria: any = {
    libraryCategory:  ["תנ''ך"],
    libraryType: [], 
    LibrarySection:  [],
    LibraryChepter:  [],
  };
  search: any = {
    Page: 1,
    ItemsPerPage: 20000
  }

  ListOfSefurimTopices : Array<any> = [];
  ListOfParsha : Array<any> = [];
  ListOfYears: Array<any> = [];
 // @ViewChild('tab7') ngselect: NgSelectComponent;
 LibraryResult : string;
 contactsDataSource: Observable<any>;

  constructor(
    private modalService: BsModalService,
    public modalRef: BsModalRef,
    public util: UtilService,
    private api: ApiService,
  ) { }

  ngOnInit(): void {
    this.onClose = new Subject();


    this.contactsDataSource = Observable.create((observer: any) => {
      observer.next(this.NewMaamar.title);
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


    this.loadParshas();
    this.loadYears();
    this.LoadTopicesDetails();
    
    var event = this.util.GetDateForYearAndParsha("תשפ״א", "תזריע");
  //  console.log('test', event);
   // console.log('NewMaamar', this.NewMaamar);
  }

  selectContact(e: TypeaheadMatch, tab7): void {
 //   console.log('selectContact', e);
  // this.masterContact.father = e.item;
   // this.LibraryResult = e.item.libraryId;
    this.NewMaamar.title = e.item.parsedText
    this.NewMaamar.LiberyTitleId.LibraryId = e.item.libraryId
    tab7.open()
   /* this.onClose.next({
      result:  this.LibraryResult,
    });
    this.modalRef.hide()*/
  }

  contactChanged() {
   // console.log('contactChanged', this.LibraryResult);
   
    //console.log(this.masterContact.father);
  }

  click(tab3, pop, tab7) {
   // console.log('maamartype',this.NewMaamar.Type)

    if (this.NewMaamar.Type  == 'PisguminKadishin') {
  //    console.log('tab3', tab3);
      tab3.open()
      setTimeout(() => {
        tab3.focus();
      })
    } else if (this.NewMaamar.Type == 'Personals') {
      tab7.open()
  //    console.log('tab7', tab7);
    } else if (this.NewMaamar.Type) {
      //pop.focus()
    //  console.log('pop', pop.popover.elementRef);
      setTimeout(() => {
        pop.popover.elementRef.nativeElement.previousElementSibling.focus();
        pop.show();
     //   console.log('open');
      }, 200)
    }
  }

  
  PersonelsOpenDate(pop) {
    setTimeout(() => {
        pop.popover.elementRef.nativeElement.previousElementSibling.focus();
        pop.show();
        console.log('open');
      }, 200)
    
  }

OpenLiberySearch(contactLkp){
 // console.log('contactLkp',contactLkp)
  contactLkp.show()
}

  DateClick(tab7, tab8, pop, tab10){
   if (!this.hidden()&& this.NewMaamar.Type == 'HadrachosYesharos') { 
    pop.hide();
 //   console.log('test hadroches yashores', tab10)
    tab10.focus()
   } else {
    tab8.focus()
   }
  }

  loadParshas(){
    this.api.getParshas(success => {
      this.ListOfParsha = success
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadYears(){
    this.api.getAllYears(success => {
      this.ListOfYears = success
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  LoadTopicesDetails(){
    this.search.CategoryId = 8
    this.api.TopicesList(this.search, success => {
      this.ListOfSefurimTopices = success.list;
  //   console.log('sefurim', this.ListOfSefurimTopices)
    }, error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    })
  }

 AddSaferToMaamar(){
   console.log('selected Safer', this.AddSafer)


   ///////////////////take this off on 11/17/2021
 /* this.NewMaamar.subTopics.push({
    topic: this.AddSafer,
    status: 'Active'
  });*/
///////////////////////////////

 // console.log('safer add to maamar', this.NewMaamar)
 }
  
  // updateType(type){
  //   this.NewMaamar.Type = type.name;
  // }

  // updatetopic(topic){
  //   this.NewMaamar.Topic = topic;
  // }

  // updatesource(source){
  //   this.NewMaamar.Source = source;
 // }

  Save(details){

    ///////////////add this 11/17/2021

    console.log('add safer', this.AddSafer)
    if (this.AddSafer.length > 0) {

      this.AddSafer.forEach(e => {
        if (e.topicID) {
          this.NewMaamar.subTopics.push({
            topic: e,
            status: 'Active'
          });
        }
  
        else{
          var topice = {
            name: e.label
          }
  
          this.NewMaamar.subTopics.push({
            topic: topice,
            status: 'Active'
          });
        }
      });
  
    }
   

    console.log('add source', this.AddSource)

    if (!this.AddSource.sourceID) {
        var NewSource = {
          firstName : this.AddSource.label
        }
        this.NewMaamar.Source = NewSource
    }
    else  {
      this.NewMaamar.Source = this.AddSource
    }


////////////////////////

   // console.log('save', this.NewMaamar);

    //validate
    if(this.NewMaamar.Type == 'PisguminKadishin') {

      var messages = '';
      if(!this.NewMaamar.title) {
        messages += 'אנא הזן כותרת, ';
      }
      if(!this.NewMaamar.parsha) {
        messages += 'אנא בחר פרשה, ';
      }
      if(!this.NewMaamar.year) {
        messages += 'אנא בחר שנה, ';
      }
      if(!this.NewMaamar.weeklyIndex) {
        messages += 'אנא בחר הזמנה, ';
      }

      if(messages) {
        this.util.openDeleteSource(messages)
        return;
      }

      //get date from year and parsha
      var event = this.util.GetDateForYearAndParsha(this.NewMaamar.year, this.NewMaamar.parsha);
     // console.log('event', event);
      if(event) {
        this.NewMaamar.date = event.getDate().greg();
      }
    }

    this.NewMaamar.maamarParagraphs = [{
      Text: '',
      ParagraphType: 0
    }];
console.log('test', this.NewMaamar)
    this.util.loadingStart();
    this.api.AddMaamar(this.NewMaamar, success => {

      this.util.loadingStop();
    if (details) {

      this.util.navigate('maamarim/' + success.maamarID)
      this.modalRef.hide()
    }

      this.onClose.next({
        data: success
      });
      this.modalRef.hide()
    }, error => {
      this.AddSafer = null;
      this.AddSource   = null;
      this.util.loadingStop();
      console.log(error)
      this.util.openDeleteSource(error.messages)
     // this.openDeleteMaamar(error.messages)

    })
  }


  hidden(){
  //console.log('this.NewMaamar.Type', this.NewMaamar.Type)
   return this.NewMaamar.Type?.startsWith("BH_") ;
   }


  // openDeleteMaamar(msg) {
  //   var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal",  initialState: {
  //     contact:{
  //       Msg: msg? msg: '?האם אתה בטוח שהנך רוצה למחוק את מאמר לצמיתות',
  //       Del: msg? false   :true
  //    }
  //          }})

  //   modalRef.content.onClose.subscribe(result => {
  //     console.log('result delete',result)
  //     if (result.success) {
  //       //this.DeleteMaamar(maamarId)
  //     }

  // })
  // }

}
