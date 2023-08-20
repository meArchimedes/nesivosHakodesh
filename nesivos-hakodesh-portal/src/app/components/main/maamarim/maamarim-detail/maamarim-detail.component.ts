import { Component, ElementRef, HostListener, NgZone, OnInit, QueryList, ViewChild, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { mergeMap } from 'rxjs/operators';
import { TypeaheadMatch } from 'ngx-bootstrap/typeahead/public_api';
import { MergePopupComponent } from '../merge-popup/merge-popup.component';
import { DeleteMaamarComponent } from '../delete-maamar/delete-maamar.component';
import { LinkMaamarComponent } from '../../sefurim/link-maamar/link-maamar.component';
import { ComponentCanDeactivate } from 'src/app/modules/can-go-back.guard';

import * as CustomDecoupledEditor from '../../../shared/CustomCkEditor/ckeditor';
import { LibraySearchComponent } from '../libray-search/libray-search.component';

@Component({
  selector: 'app-maamarim-detail',
  templateUrl: './maamarim-detail.component.html',
  styleUrls: ['./maamarim-detail.component.css']
})
export class MaamarimDetailComponent implements OnInit, ComponentCanDeactivate {
  public tabs = Tabs;
  public activeTab: number = 2;

  search: any = {
    Page: 1,
    ItemsPerPage: 20000
  }
  searchCriteria: any = {
    libraryCategory:  ["תנ''ך"],
    libraryType: [], 
    LibrarySection:  [],
    LibraryChepter:  [],
  };

  CategorySearch: any = {
    Page: 1,
    ItemsPerPage: 2000,
    sortBy: 'CategoryName',
    sortDirection: 'Ascending',

   }
   ListOfCategories : any = []
  maamarId;
  Maamar : any = {
    date: new Date(),
     maamarParagraphs: [],
     source : {},
    subTopics: [],
     topic: {},
    // torah: {}
    updatedUser: {},
    liberyTitleId:{}
  };
  saveMaamarJson: string = '';
  color: string

  Topics: Array<any> = [];
  TorahLinks: Array<any> = [];
  selectedSource: string= '';
  Source
  contactsDataSource: Observable<any>;
  contactsLiberySource: Observable<any>;
  backToSearchResult: boolean = false;

  typeOptions = [{color: 'red', text: 'פתגמין קדישין', value: 'PisguminKadishin'},
               {color: 'blue', text: 'הדרכות ישרות', value: 'HadrachosYesharos'},
               {color: 'green', text: 'הדרכות פרטיות', value: 'Personals'},
               {color: 'green', text: 'נועם שיח', value: 'BH_NoamSheikh'},
               {color: 'green', text: 'שיחות קודש', value: 'BH_SichesKodesh'},
               {color: 'green', text: 'ברכות קודש והמסתעף', value: 'BH_BerchesKodesh'},
               {color: 'green', text: 'מאמרים פרטיים', value: 'BH_MaameremProtem'},
               {color: 'green', text: 'סיפורי קודש', value: 'BH_SeperiKodesh'}]



  statusOptions = [
                  //{color: 'tag tag-red', text: 'חדש בלי תוכן', value: 'NewWithOutDetails'},
                 // {color: 'tag tag-purple', text: 'יש למלאות פרטים וקישורים', value: 'NeedDetails'},
                  {color: 'tag tag-yellow', text: 'עדיין לא מוגה', value: 'NotMuggedYet'},
                  {color: 'tag tag-light-Green', text: 'יש עבודה עליו', value: 'NeedToWork'},
                  {color: 'tag tag-green', text: 'מושלם', value: 'Perfect'}];


  ListOfParsha : Array<any> = [];
  ListOfYears: Array<any> = [];

  @ViewChildren('editableParagraph') editableParagraphs: QueryList<ElementRef>;
  @ViewChild('audio') audio: ElementRef;
  @ViewChild('aLinkPdf') aLinkPdf: ElementRef;
  @ViewChild('aLinkWord') aLinkWord: ElementRef;
  @ViewChild('aLinkAudio') aLinkAudio: ElementRef;

  public Editor = CustomDecoupledEditor;
  editor2;
  edtrConfig = { 
    licenseKey: '3y63aq8CrwhJ/nGkoeFUQ1nMo7i9Br785ExyNAKdXOOsHmMwBdLJXxzs',
   language: {
      ui: 'en',
      content: 'ar'
    },
    cusMssSAett: {
      
			GetLink: (text, cb) => {

        var modalRef = this.modalService.show(LibraySearchComponent, { class: "merge-modal-popup ngDraggable" , backdrop:'static', initialState: {
          LibraryResult: text.trim()
        }});
    
        modalRef.content.onClose.subscribe(result => {
        var link = 'library/' + result.result
          cb(link);
      }) 
			}
		}
  };
  

  constructor(
    public util: UtilService,
    private route: ActivatedRoute,
    private api: ApiService,
    private router: Router,
    private modalService: BsModalService
  ) { }

  /*dragImageOffsetRight: DndDragImageOffsetFunction = ( event:DragEvent, dragImage:Element ) => {
    const dragImageComputedStyle = window.getComputedStyle( dragImage );
    const paddingTop = parseFloat( dragImageComputedStyle.paddingTop ) || 0;
    const paddingLeft = parseFloat( dragImageComputedStyle.paddingLeft ) || 0;
    const borderTop = parseFloat( dragImageComputedStyle.borderTopWidth ) || 0;
    const borderLeft = parseFloat( dragImageComputedStyle.borderLeftWidth ) || 0;

    const x = dragImage.clientWidth - (event.offsetX + paddingLeft + borderLeft);
    return {
      x: x,
      y: event.offsetY + paddingTop + borderTop
    };
  };*/

  ngOnInit(): void {

    // var modalRef = this.modalService.show(LibraySearchComponent, { class: "merge-modal-popup" , initialState: {
      this.contactsLiberySource = Observable.create((observer: any) => {
        observer.next(this.Maamar.title);
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
          
    // }});


    this.maamarId = this.route.snapshot.paramMap.get('id');

    //load or take from saved
    /*var savedFromList = this.util.GetAndDeleteSheardData('maamar-' +  this.maamarId);
    //console.log('ext', ext);

    if(savedFromList.hasOwnProperty("maamarID")) {
      var savedFromListCopy = this.util.CopyObject(savedFromList);
      //console.log('savedFromList', savedFromList.content);
      //console.log('savedFromListCopy', savedFromListCopy.content);
      this.afterLoadMaamar(savedFromList);
      this.LoadTorhaLinks();
    } else {
        this.LoadMaamarDetails()
    }*/

    this.LoadMaamarDetails();
    this.contactsDataSource = Observable.create((observer: any) => {
      observer.next(this.selectedSource);
    }).pipe(mergeMap((token: string) => this.api.SearchSource(token)));

    this.loadParshas();
    this.loadYears();
    this.LoadTopicesDetails();
    this.LoadListOfCategories()

    this.route.queryParams.subscribe(val => {
      //  console.log('subscribe', val);
      if(val.BackToSearch == 'true') {
        this.backToSearchResult = true;
      }
    });
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

  selectContact(e: TypeaheadMatch): void {
    this.Maamar.source = e.item;
    this.selectedSource = e.value;
  }

 

  selectLiberyContact(e: TypeaheadMatch): void {
 //   console.log('selectLiberyContact', e);
  // this.masterContact.father = e.item;
   // this.LibraryResult = e.item.libraryId;
    this.Maamar.title = e.item.parsedText
    
    
    this.Maamar.liberyTitleId = e.item
  //  console.log(' this.Maamar.LiberyTitleId',  this.Maamar.liberyTitleId)
  //  console.log(' this.Maamar',  this.Maamar)
   /* this.onClose.next({
      result:  this.LibraryResult,
    });
    this.modalRef.hide()*/
  }


  addTagPromise(name) {
  }


  hidden(Maamar){
    //console.log('this.NewMaamar.Type', Maamar.type)
     return Maamar.type?.startsWith("BH_") ;
     }

  onReady(editor ) {

    this.editor2 = editor;
    //this.LoadMaamarDetails();
  //  console.log('on ready', editor );
    editor .ui.getEditableElement().parentElement.insertBefore(
      editor .ui.view.toolbar.element,
      editor .ui.getEditableElement()
    );

    //this.populateComments(editor);
    
  }

  populateComments(editor, maamar) {

    //console.log('populateComments - editror', editor);
    //console.log('populateComments - maamar', maamar);


    const usersPlugin = editor.plugins.get( 'Users' );
    const commentsRepository = editor.plugins.get( 'CommentsRepository' );

    
    //console.log('this.util.getUserList()', this.util.getUserList());
    //console.log('this.util.getCurrentUser()', this.util.getCurrentUser());

  //  console.log('usersPlugin', usersPlugin.users.length);

    if(usersPlugin.users.length == 0) {
      for ( const user of this.util.getUserList() ) {
          usersPlugin.addUser({
            id: `${user.id}`,
            name: user.fullName
          });
      }

      // Set the current user.
      usersPlugin.defineMe(`${this.util.getCurrentUser().id}`);
    }


    
    if(maamar.comments) {

      var commentsArray = JSON.parse(maamar.comments);
    //  console.log('commentsArray', commentsArray);


      // Load the comment threads data.
      for ( const commentThread of commentsArray) {

        if(!commentsRepository.hasCommentThread(commentThread.threadId)) {
          commentsRepository.addCommentThread( commentThread );
        }
      }
    }

    if(maamar.content != null) {
     // console.log('set data to editor');
      editor.setData(maamar.content);
    }

    

  }

handleUpload(e, type):void{

  //console.log('handleUpload', e.target.value);
  if(e.target.files.length) {

    var subType = type == 'originalFileName' ? 'word' : type == 'pdfFileName' ? 'pdf' : 'audio';

    this.util.loadingStart();
    this.api.UploadFile('Maamarim', this.maamarId, subType, e.target.files[0], success => {

      this.Maamar[type] = success;
      this.Save();
      e.target.value = '';

    }, error => {

      console.log(error);
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
      e.target.value = '';
    });

  }
}

removeFile(type, e) {

  e.stopPropagation();

 // console.log('revmove file', type);

  var modalRef =  this.modalService.show(DeleteMaamarComponent, {
    class: "delete-maamar-modal ngDraggable" , backdrop:'static',
    initialState: {
      contact: {
        Msg: '?האם אתה בטוח שהנך רוצה למחוק את קובץ מקור',
        Del: true
      }
    }
  });

  modalRef.content.onClose.subscribe(result => {

  //  console.log('result delete',result)

    if (result.success) {

      this.Maamar[type] = null;
      this.Save();
    }
  });
}

getFileUrl(name) {
  var safeUrl = this.util.sanitizeUrl('MssLocalFile://Maamarim//' + name);
  return safeUrl;
}

openPdfFile() {
  this.aLinkPdf.nativeElement.click();
}
openWordFile() {
  this.aLinkWord.nativeElement.click();
}
openAudioFile() {
  this.aLinkAudio.nativeElement.click();
}

LoadMaamarDetails(){
  this.util.loadingStart();

  this.api.Getmaamar(this.maamarId, success => {

    console.log('success', success);
   // console.log('torah link before', this.TorahLinks);

    if(this.editor2 && success.content) {
      this.populateComments(this.editor2, success);
    }

    this.afterLoadMaamar(success);

    // switch (success.status) {
    //   case 'NewWithOutDetails':
    //    this.color = 'tag tag-red';
    //     break;
    //     case 'NeedDetails':
    //     this.color = 'tag tag-purple';
    //     break;
    //     case 'NotMuggedYet':
    //     this.color = 'tag tag-yellow';
    //     break;
    //   case 'NeedToWork':
    //     this.color = 'tag tag-light-Green';
    //     break;
    //   case 'Perfect':
    //     this.color = 'tag tag-green';

    // }

   // this.color = 'tag tag-purple'
    this.util.loadingStop();
  }, error => {
    //this.util.showError(error.messages)
    this.util.openDeleteSource(error.messages)
    this.util.loadingStop();
  })
}

  afterLoadMaamar(maamar) {

    this.Maamar = maamar;
    /*if (this.Maamar.maamarParagraphs.length == 0) {
      this.AddParagraph(-1, 0)
    }*/
    if (this.Maamar.source != null) {
      this.selectedSource =  this.Maamar.source.fullName;

     this.Source =  this.Maamar.source.firstName;
    }

    //audio
    if(this.Maamar.audioFileName) {
      //console.log('has audio');

      this.api.ReadFile(this.Maamar.audioFileName, 'Maamarim').then(success => {

        var audioFileSrc = success['body'];
        const blob = new Blob([audioFileSrc], { type: 'audio/wav' });
        const url = URL.createObjectURL(blob);

        this.audio.nativeElement.src = url;
        this.audio.nativeElement.load();

      });
    }

    this.saveBackup();
  }

  LoadTorhaLinks(){

    this.api.Getmaamar(this.maamarId, success => {

      this.Maamar.torahLinks = success.torahLinks;
      this.saveBackup();

    }, error => {
      console.log(error);
    })
  }

  saveBackup() {
    this.Maamar.editable = false;
    this.saveMaamarJson = JSON.stringify(this.Maamar);

    if(!this.Maamar.content || this.Maamar.content.trim().length == 0) {
      this.Maamar.editable = true;
    }
    //console.log('save backup', this.Maamar.content);
  }

  LoadTopicesDetails(){

    this.api.TopicesList(this.search, success => {
      this.Topics = success.list;
    // console.log('topices', this.Topics)
    }, error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    })
  }

  onPaste(event: ClipboardEvent, i) {

    let clipboardData = event.clipboardData;
    let pastedText = clipboardData.getData('text');
    var spl = pastedText.split('\n');

    var filtered = spl.filter(function (el) {
      return el != null && el.trim() != '';
    });

    //console.log('onPaste ' + i, filtered);

    this.Maamar.maamarParagraphs[i].text = filtered[0];

    for (let index = 1; index < filtered.length; index++) {

      var newP = {
        paragraphType: 0,
        text: filtered[index]
      };
      this.Maamar.maamarParagraphs.splice(i + index, 0, newP);
    }

    setTimeout(() => {
      this.editableParagraphs.toArray()[i + filtered.length - 1].nativeElement.focus();
    },100);

    return false;
  }

  AddParagraph(i, type){


    var newP = {
      paragraphType: type,
    };

    this.Maamar.maamarParagraphs.splice(i + 1, 0, newP);

    setTimeout(() => {
      this.editableParagraphs.toArray()[i + 1].nativeElement.focus();
    },100);


  }

  // GetParagraph(){
  //   return this.Maamar.maamarParagraphs.filter(x => !x.isDeleted)
  // }



  DeleteParagraph(i) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable" , backdrop:'static', initialState: {
      contact:{
        Msg:  '?האם אתה בטוח שהנך רוצה למחוק את פסקה לצמיתות',
        Del:    true
     }
           }})

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {
        i.status =  1;
        this.Save();
      }

  })
  }



updateType(so){
  this.Maamar.type = so.value;
  this.Maamar.typeValue = so.text;
}

  updateStatus(so) {
    this.Maamar.status = so.value;
    this.Maamar.statusValue = so.text;

  }


updateTopic(so) {
  this.Maamar.topic = so

}


    getMainSefer() {
     return this.Maamar.subTopics.filter(x => x.status != 'Deleted' && x.topic.category.categoryName == 'ספרים' && x.mainTopic == true)
    }


  updateMainSefer(so) {
   // console.log('so topice', so)
    var currMainSefer = this.getMainSefer();
    console.log('Curr main Sefer', currMainSefer)

    // make sure its not null
    /*if (currMainSefer.length > 0) {
      currMainSefer[0].status = 'Deleted'
    }*/
    
    this.Maamar.subTopics.push({
      topic: so,
      status: 'Active',
      mainTopic: true
    });

  }
  isMainSeferTopicActive(so) {
    return this.getMainSefer().filter(x => x.topic.topicID === so.topicID).length > 0;
  }


StatusColor(Maamar){

  var colorDetails:any = {};

  switch (Maamar.status) {
    case 'NewWithOutDetails':
      colorDetails.color = 'tag tag-red';
      colorDetails.ChangeMenu = true;
      break;
      case 'NeedDetails':
        colorDetails.color = 'tag tag-purple';
        colorDetails.ChangeMenu = true;
      break;
      case 'NotMuggedYet':
        colorDetails.color = 'tag tag-yellow';
        colorDetails.ChangeMenu = false;
      break;
    case 'NeedToWork':
      colorDetails.color = 'tag tag-light-Green';
      colorDetails.ChangeMenu = false;
      break;
    case 'Perfect':
      colorDetails.color = 'tag tag-green';
      colorDetails.ChangeMenu = false;

  }
  return colorDetails;
}

getSubTopics(CategoryName = null) {

if (CategoryName != null) {
  return this.Maamar.subTopics.filter(x => x.status != 'Deleted' && x.topic.category.categoryName == CategoryName && x.mainTopic != true)
}
else  {
  return this.Maamar.subTopics.filter(x => x.status != 'Deleted' )
}
 

}
isSubTopicActive(so) {
  return this.getSubTopics().filter(x => x.topic.topicID === so.topicID).length > 0;
}

  updateSubTopic(so) {
    //console.log('so topice', so)
    var curr = this.getSubTopics().filter(x => x.topic.topicID === so.topicID);

    if(curr.length > 0) {

      curr[0].status = 'Deleted';

    } else {
      this.Maamar.subTopics.push({
        topic: so,
        status: 'Active'
      });
    }
  }

  Save(){

    var cc = this.editor2.getData();
    console.log('Save contacts', cc);
    this.Maamar.content = this.editor2.getData();

    const commentsRepository = this.editor2.plugins.get( 'CommentsRepository' );
    const commentThreadsData = commentsRepository.getCommentThreads( {
      skipNotAttached: true,
      skipEmpty: true,
      toJSON: true
    } );
  //  console.log('commentThreadsData', commentThreadsData);
    this.Maamar.comments = JSON.stringify(commentThreadsData);

    this.util.loadingStart();

    //save year based on date
   // console.log('Save', this.Maamar);
    this.api.UpdateMaamar(this.Maamar, success => {
      this.LoadMaamarDetails()
      this.util.loadingStop();
      this.util.showToast();
    }, error => {
    //  console.log('error update', error)
     // this.util.showError(error.messages)
     this.util.openDeleteSource(error.messages)
      this.util.loadingStop();
    })
  }

  Back() {

    //
    if(this.backToSearchResult) {

      this.util.navigate('search-results');

    } else {
      this.util.navigate('maamarim');
    }
  }

  DeleteMaamar(){

      this.api.DeleteMaamar(this.maamarId, success =>{
        this.router.navigate(['/maamarim' ])
      }, error => {
        console.log('error', error)
       // this.util.showError(error.messages)
       this.util.openDeleteSource(error.messages)
      })

  }



  // openDeleteMaamar() {
  //   var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal" , initialState: {
  //     contact: {
  //       Msg: '?האם אתה בטוח שהנך רוצה למחוק את מאמר לצמיתות',
  //       Del: true
  //     }
  //   }})

  //   modalRef.content.onClose.subscribe(result => {
  //     console.log('result delete',result)
  //     if (result.success) {
  //       this.DeleteMaamar()
  //     }

  // })
  // }

  /*onDrop( event:DndDropEvent, list?:any[] ) {
    if (list && (event.dropEffect === "copy" || event.dropEffect === "move")) {
      let index = event.index;

      if (typeof index === "undefined") {
        index = list.length;
      }

      list.splice(index, 0, event.data);
    }
  }

  onDragged(item: any, list: any[], effect: DropEffect) {
    if(effect === "move") {
      const index = list.indexOf(item);
      list.splice(index, 1);
    }
  }*/

  deleteTorahLink(TorahLink, e){

    if(e != null) {
      e.stopPropagation();
    }
    //console.log('maamarTorahLinkID', TorahLink)
    this.api.DeleteMaamarTorahLink(TorahLink.maamarTorahLinkID, success => {

      this.activeTab = this.tabs.links
      this.TorahLinks = [];
      this.LoadMaamarDetails();
    }, error => {
      this.util.openDeleteSource(error.messages)
    })
  }

  openMaamarLink(MaamarLink){
    var id = MaamarLink.maamar.maamarID;
    this.util.navigateNewWindow(`/maamarim/${id}`);
  }

  getMaamarimLinkToTorah(TorahLink){
    TorahLink.open = !TorahLink.open
    //console.log('work link!', TorahLink)
    this.api.GetMaamarTorahLink(TorahLink.torah.torahID, success => {

        this.TorahLinks = success.filter(x => x.maamar.maamarID !=  this.Maamar.maamarID)
      // console.log('GetMaamarTorahLink', this.TorahLinks)
    }, error => {
      this.util.openDeleteSource(error.messages)
    })
  }

  openDeleteMaamar(MaamarLink = null, event = null) {

    if(event != null) {
      event.stopPropagation();
    }

    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable" , backdrop:'static', initialState: {
      contact: {
        Msg: '?האם אתה בטוח שהנך רוצה למחוק את מאמר לצמיתות',
        Del: true
      }
    }})

    modalRef.content.onClose.subscribe(result => {
     // console.log('result delete',result)
      if (result.success) {
        if (MaamarLink == null) {
          this.DeleteMaamar()
        } else {
          this.deleteTorahLink(MaamarLink, event);
        }

      }

  })
  }



  openTorahLink(TorahLink){
    //console.log('TorahLink', TorahLink);
    var urlString = `/sefurim/${TorahLink.torah.sefer.seferID}?torah=${TorahLink.torah.torahID}`;
    this.util.navigateNewWindow(urlString);
  }

  openMergeModal() {
    var modalRef = this.modalService.show(MergePopupComponent, { class: "merge-modal ngDraggable" , backdrop:'static', initialState: {
      Maamar: this.Maamar

    }});
    modalRef.content.onClose.subscribe(result => {
     // console.log('result Merge Pop Up',result)

       this.LoadMaamarDetails()


  })
  }

  openMergeModal2() {

    var modalRef = this.modalService.show(LinkMaamarComponent, { class: "merge-modal ngDraggable" , backdrop:'static', initialState: {
      Maamar: this.Maamar
    }});
    modalRef.content.onClose.subscribe(result => {

       this.LoadMaamarDetails();
    })
  }

  setDate(e) {
   // console.log('SetDate', e);
    if(e.success) {

      this.Maamar.date = e.englishDate;
      this.Maamar.parsha = e.hebrewParsharName;
      this.Maamar.year = e.hebrewYearName;

    } else {
      this.Maamar.date = null;
      this.Maamar.parsha = null;
      this.Maamar.year = null;
    }
  }

  updateDate() {

    var event = this.util.GetDateForYearAndParsha(this.Maamar.year, this.Maamar.parsha);
   // console.log('event', event);
    if(event) {
      this.Maamar.date = event.getDate().greg();
    }
  }

  controlClicked: boolean = false;
  @HostListener('window:keyup', ['$event'])
  keyEvent(event: KeyboardEvent) {

    //console.log('HostListener', event);
    if(event.key == 'Control') {

      this.controlClicked = true;
      setTimeout(() => {
        this.controlClicked = false;
      }, 100);
    }
    if(event.code == 'KeyS') {
      if(this.controlClicked) {

        event.preventDefault();
        event.stopPropagation();
       // console.log('save data');
        this.Save();
        return false;
      }
    }

  }

  canDeactivate() {

    
    this.Maamar.editable = false;
    this.Maamar.content = this.editor2.getData();
    this. Maamar.torahLinks.forEach(x => {
    //  console.log('x', x)
      delete x['open'];
      //x.open = false
    });
    var newJson = JSON.stringify(this.Maamar);

    if(newJson != this.saveMaamarJson) {
     // console.log('old', this.saveMaamarJson);
     // console.log('new', newJson);
      return false;
    }

    return true;
  }

  saveAndGoBack(successCallback) {

   // console.log('save and go back', this.Maamar);

    this.util.loadingStart();
    this.api.UpdateMaamar(this.Maamar, success => {

      this.util.loadingStop();
      this.util.showToast();

      successCallback(true);

    }, error => {

      console.log('error update', error)
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)

      successCallback(false);
    })


  }

  // All types of categoreis

  LoadListOfCategories(){
    this.util.loadingStart();
    this.api.GetCategoryList(this.CategorySearch, success => {
     // console.log('ListOfCategories success',success)
    this.ListOfCategories = success.list
   
   // console.log('ListOfCategories Maamar details', this.ListOfCategories)
    this.util.loadingStop();
    }, error => {
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
    })
  }

 getTopicesGroupByCategories(CategoryName){
 return  this.Topics.filter(x => x.category?.categoryName == CategoryName)

 }


 ChangeSource(event){
console.log('ChangeSource', event)
this.Maamar.Source = event
  
 }
}

enum Tabs {
  sources,
  links,
  generalInfo
}
