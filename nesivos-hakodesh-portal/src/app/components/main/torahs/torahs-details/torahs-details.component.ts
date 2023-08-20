import { Component, ElementRef, OnInit, QueryList, Renderer2, ViewChildren } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { error } from 'protractor';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { MaamarimListComponent } from '../../maamarim/maamarim-list/maamarim-list.component';
import { MergeMaamarPopUpComponent } from '../merge-maamar-pop-up/merge-maamar-pop-up.component';
//import BalloonEditor from '@ckeditor/ckeditor5-build-balloon';

@Component({
  selector: 'app-torahs-details',
  templateUrl: './torahs-details.component.html',
  styleUrls: ['./torahs-details.component.css']
})
export class TorahsDetailsComponent implements OnInit {
  public tabs = Tabs;
  public activeTab: number = 1;
  TorahID
  TorahParagraphs : any = {
    torahParagraphs: []
  };
  paragraph : any = {}
  obListOfParsha: any = [];
  search: any = {
    Page: 1,
    ItemsPerPage: 50,
    
  }
  ListOfSefurim: Array<any> = [];


  @ViewChildren('editableParagraph') editableParagraphs: QueryList<ElementRef>;
  
  /*public Editor = BalloonEditor;
  edtrConfig = { 
    toolbar: [ 'heading', '|', 'bold', 'italic' ] ,
    language: {
      ui: 'en',
      content: 'ar'
    }
  };*/

  constructor(
    private _renderer: Renderer2,
    private route: ActivatedRoute,
    private api: ApiService,
    public util: UtilService,
    private modalService: BsModalService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.TorahID = this.route.snapshot.paramMap.get('id');
    this.LoadTorahDetails()
  }


  LoadTorahDetails(){
    this.util.loadingStart()
  this.api.GetTorah(this.TorahID, success => {
  //  console.log('hhhh', success)
   this.util.loadingStop()
   this.TorahParagraphs = success
   if (this.TorahParagraphs.torahParagraphs.length == 0) {
    this.AddParagraph(-1)
  }
   this.loadParshas();
   this.loadSefurim();
  //this.paragraph = this. TorahParagraphs.torahParagraphs
  }, error => {
    this.util.loadingStop()
    this.util.openDeleteSource(error.messages)
  })
  }

  loadSefurim(){
   
    this.api.SefurimList(this.search, success => {
     this.ListOfSefurim = success.list;
    }, error => {
       console.log('error', error)

     //  this.util.showError(error)
     this.util.openDeleteSource(error.messages)
    })
  }

  loadParshas(){
   

    this.api.getParshas(success => {
   
    //this.ListOfParsha = success
    
    success.forEach(element => {
 
      this.obListOfParsha.push({
            name: element
      }) });
     }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  updateSefer(so){
    this.TorahParagraphs.sefer = so
  }

  updateParsha (so) {

    this.TorahParagraphs.parsha = so.name
  
  }
  
  openMergeModal() {
    var modalRef = this.modalService.show(MergeMaamarPopUpComponent, { class: "merge-modal ngDraggable" ,backdrop:'static', initialState: {
      thora: this.TorahParagraphs
    }});
    
    modalRef.content.onClose.subscribe(result => {
     // console.log('result Merge Pop Up',result)
      this.LoadTorahDetails()

      if(result != '') {
        
      }

  })
  }

  openMaamarLink(MaamarLink){
    var id = MaamarLink.maamar.maamarID;
    this.util.navigateNewWindow(`/maamarim/${id}`);
  }

  handleUpload(e):void{

    if(e.target.files.length) {
  
  
      this.util.loadingStart();
      this.api.UploadFile('Torahs', this.TorahID, '', e.target.files[0], success => {
  
        this.TorahParagraphs.originalFileName = success;
        this.Save();
  
      }, error => {
  
        console.log(error);
        this.util.loadingStop();
        this.util.openDeleteSource(error.messages)

      });
  
    } else {
  
      //need to be able to remove the file
    }
  }

  onPaste(event: ClipboardEvent, i) {

    let clipboardData = event.clipboardData;
    let pastedText = clipboardData.getData('text');    
    var spl = pastedText.split('\n');
    
    var filtered = spl.filter(function (el) {
      return el != null && el.trim() != '';
    });

    //console.log('onPaste ' + i, filtered);

    this.TorahParagraphs.torahParagraphs[i].text = filtered[0];
    
    for (let index = 1; index < filtered.length; index++) {
      
      var newP = {
        text: filtered[index]
      };
      this.TorahParagraphs.torahParagraphs.splice(i + index, 0, newP);
    }

    setTimeout(() => {
    //  console.log(i + filtered.length - 1);
      this.editableParagraphs.toArray()[i + filtered.length - 1].nativeElement.focus();
    },100);

    return false;
  }

  AddParagraph(i){


    var newP = {
      
    };
    this.TorahParagraphs.torahParagraphs.splice(i + 1, 0, newP);

    setTimeout(() => {
      this.editableParagraphs.toArray()[i + 1].nativeElement.focus();
    },100);


  }

  Save(){

    this.util.loadingStart();
    this.api.UpdateTorah(this.TorahParagraphs, success => {
      this.LoadTorahDetails();
      this.util.loadingStop();
    }, error => {
      console.log('error update', error)
     // this.util.showError(error.messages)
     this.util.openDeleteSource(error.messages)
      this.util.loadingStop();
    })
  }

  DeleteParagraph(Paragraph) {
    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable" , backdrop:'static', initialState: {
      contact:{
        Msg:  '?האם אתה בטוח שהנך רוצה למחוק את פסקה לצמיתות',
        Del:    true
     }
           }})

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {

        Paragraph.isDeleted = true;
        this.Save();
      }

  })
  }

  deleteMaamarLink(MaamarLink){

    this.api.DeleteMaamarTorahLink(MaamarLink.maamarTorahLinkID, success => {
    //  console.log(success)
      this.LoadTorahDetails()
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
      if (result.success) {
        if (MaamarLink == null) {
          this.DeleteTorah()
        } else {
          this.deleteMaamarLink(MaamarLink);
        }

      }

  })
  }

  DeleteTorah(){
    this.api.DeleteTorah(this.TorahParagraphs.torahID, success => {
      this.router.navigate(['/torahs'])
    }, error => {
      this.util.openDeleteSource(error.messages)
    })
  }
}

enum Tabs {
  links,
  generalInfo
}
