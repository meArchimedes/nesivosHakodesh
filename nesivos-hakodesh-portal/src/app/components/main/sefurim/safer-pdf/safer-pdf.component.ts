import { Component, EventEmitter, Input, NgZone, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ComponentCanDeactivate } from 'src/app/modules/can-go-back.guard';

import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { PdfViewAnnotation, PdfViewRectangle, ShapeType } from 'src/app/shared/pdf-view/pdf-view-annotation';
import { DeleteMaamarComponent } from '../../maamarim/delete-maamar/delete-maamar.component';
import { MergeMaamarPopUpComponent } from '../../torahs/merge-maamar-pop-up/merge-maamar-pop-up.component';

@Component({
  selector: 'app-safer-pdf',
  templateUrl: './safer-pdf.component.html',
  styleUrls: ['./safer-pdf.component.css']
})
export class SaferPdfComponent implements OnInit, ComponentCanDeactivate   {

  @Input() linkSaferId: number = 0;
  @Output() selectionUpdated = new EventEmitter<any>();

  public tabs = Tabs;
  public activeTab: number = 1;
  saferId;
  safer: any = {};
  pdfSrc;
  selectedTorah: any = {};
  obListOfParsha: any = [];
  page;
  selectedTorahId;
  saveJson: string = '';
  annotations: Array<any> = [];

  constructor(
    private api: ApiService, 
    private route: ActivatedRoute, 
    public util: UtilService,
    private zone: NgZone,
    private modalService: BsModalService,
    private router: Router,
    ) { 

      this.route.queryParams.subscribe(params => {
        //console.log('queryParams', params);
        var to = params['torah'];
        if(to) {
          this.selectedTorahId = to;
          this.selectedTorah = {torahID: this.selectedTorahId};
          this.activeTab = this.tabs.torahDetails;
        }
    });

    }

  isLinkPopup() {
    return this.linkSaferId != 0;
  }

  ngOnInit(): void {
    


    if(this.isLinkPopup()) {
      this.saferId = this.linkSaferId;
    } else {

      this.saferId = this.route.snapshot.paramMap.get('id');
    }

    //console.log('saferId', this.saferId);
    this.LoadDetails(true);
    this.loadParshas();
  }

  

  LoadDetails(loadFile){
    this.util.loadingStart()
    this.api.GetSafer(this.saferId, success => {

      this.util.loadingStop()
     // console.log('LoadDetails sefer', success);
      this.safer = success;
      this.saveJson = JSON.stringify(this.safer);

      //populate ann
      this.annotations = this.getAnnotations();

      if(this.safer.fileUrl && loadFile) {
        setTimeout(() => {
          this.loadPdfFile();
        });
      }

      this.selectedTorah = this.getSelectedTorah(this.selectedTorah);
    
    }, error => {
      this.util.loadingStop()
      this.util.openDeleteSource(error.messages)
    })
  }

  getSelectedTorah(rorah) {

    var find = this.safer.torahs.filter(x => x.torahID == rorah.torahID);
    if(find.length > 0) {
      return find[0];
    }

    return {};
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

  GoToPage(page) {
    this.page = page;
  }

  openMergeModal() {

    var to = this.util.CopyObject(this.selectedTorah);
    to.sefer = this.safer;
  //   console.log('to', to)
    var modalRef = this.modalService.show(MergeMaamarPopUpComponent, { class: "merge-modal ngDraggable" ,  backdrop:'static', initialState: {
      thora: to
    }});
    
    modalRef.content.onClose.subscribe(result => {
      
      //reload selected torha
      this.LoadDetails(false);

    })
  }

  openMaamarLink(MaamarLink){
    var id = MaamarLink.maamar.maamarID;
    this.util.navigateNewWindow(`/maamarim/${id}`);
  }

  openDeleteMaamarLink(MaamarLink, event = null) {

    if(event != null) {
      event.stopPropagation();
    }

    var modalRef =  this.modalService.show(DeleteMaamarComponent, { class: "delete-maamar-modal ngDraggable" ,backdrop:'static', initialState: {
      contact: {
        Msg: '?האם אתה בטוח שהנך רוצה למחוק את מאמר לצמיתות',
        Del: true
      }
    }})

    modalRef.content.onClose.subscribe(result => {
      if (result.success) {

        this.api.DeleteMaamarTorahLink(MaamarLink.maamarTorahLinkID, success => {
        //  console.log(success)

          this.LoadDetails(false);
        }, error => {
          this.util.openDeleteSource(error.messages)
        })
      }
    })
  }

  getHebrewDateText(date) {
    return this.util.getHebrewDateNames(date).dwy;
  }

  save() {

  //  console.log('this.safer', this.safer)
   this.safer.torahs.forEach(torah => {

    if (torah.maamarLinks) {
      torah.maamarLinks.forEach(mammarlink => {
        mammarlink.maamar.torahLinks = [];
      });
    }
   

   });
    this.util.loadingStart();
  //  console.log('upate safer', this.safer);
    this.api.UpdateSefur(this.safer, success => {
      
    //  console.log('upate after save', success);
      this.safer = success;
      this.util.loadingStop();
      this.util.showToast();
      this.saveJson = JSON.stringify(this.safer);
      this.annotations = this.getAnnotations();


      //reload file
      /*var urlBackup = this.safer.fileUrl;
      this.safer.fileUrl = '';
      setTimeout(() => {
        this.safer.fileUrl = urlBackup;
      });*/


     }, error => {
      console.log(error)
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
     })

  }

  handleUpload(e):void{

    if(e.target.files.length) {
  
      this.util.loadingStart();
      this.api.UploadFile('Sefurim', this.saferId, 'file', e.target.files[0], success => {
  
        this.safer.fileUrl = success;
        this.save();
  
        if(this.safer.fileUrl) {
          setTimeout(() => {
            this.loadPdfFile();
          });
        }

      }, error => {
  
        console.log(error);
        this.util.loadingStop();
        this.util.openDeleteSource(error.messages)

      });
    }
  }

  loadPdfFile() {
    this.api.ReadFile(this.safer.fileUrl, 'Sefurim').then(success => {
      this.pdfSrc = success['body'];
    });
  }

  getAnnotations() {

    var list: Array<PdfViewAnnotation> = [];

    this.safer.torahs.filter(x => x.status == 0).forEach(ann => {
      list.push(this.parseToPdfViewAnnotation(ann));
    });
    //console.log(list);

    return list;
  }

  parseToPdfViewAnnotation(ann) {

    var newAnn = new PdfViewRectangle();
    newAnn.id = ann.torahID;
    newAnn.shapeType = ShapeType.Rectangle;
    newAnn.x = ann.annX;
    newAnn.y = ann.annY;
    newAnn.w = ann.annWidth;
    newAnn.h = ann.annHeight;
    newAnn.pageNumber = ann.annPageNumber;
    newAnn.customData = ann;

    return newAnn;
  }

  annotationSelected(t) {
    console.log('annotationSelected', t);

    if(t) {
      this.selectedTorah = this.getSelectedTorah({torahID: t.id});
      this.activeTab = this.tabs.torahDetails;
    } else {
      this.activeTab = this.tabs.generalInfo;
      this.selectedTorah = {};
    }

    this.selectionUpdated.emit(this.selectedTorah);

    //TODO remove once we have our own viewer
    this.zone.run(() => {});
  }

  annotationChanged(e) {
  //  console.log('annotationChanged', e);

    //need to handle additions
    if(e.action == 'add') {

      this.safer.torahs.push({
        torahID: e.annotation.id,
        annPageNumber: e.annotation.pageNumber,
        annX: e.annotation.x,
        annY: e.annotation.y,
        annWidth: e.annotation.w,
        annHeight: e.annotation.h,
        status: e.annotation.customData.status,
      });
    } else {

      var torhaList = this.safer.torahs.filter(x => x.torahID == e.annotation.id);
      if(torhaList.length > 0) {

        var torha = torhaList[0];
        //update annotation data
        torha.annX = e.annotation.x;
        torha.annY = e.annotation.y;
        torha.annWidth = e.annotation.w;
        torha.annHeight = e.annotation.h;
        torha.status = e.annotation.customData.status;
      }
    }
    
    this.zone.run(() => {});
    //console.log('this.safer.torahs', this.safer.torahs);
  }

  getParentApi(): ParentComponentApi {
    return {
      callBeforeDelete: (id, cb) => {
        //console.log('callBeforeDelete', id);

        this.modalService.show(DeleteMaamarComponent, { 
          class: "delete-maamar-modal ngDraggable" , 
          backdrop:'static',
          initialState: {
            contact: {
              Msg: '?האם אתה בטוח שהנך רוצה למחוק את התורה',
              Del: true
            }
          }
        }).content.onClose.subscribe(result => {
          if (result.success) {
            cb();
          }
        });
      },
      callDelete: (id, cb) => {
      //  console.log('callDelete', id);

        if(id < 0) {

          var torhaList = this.safer.torahs.filter(x => x.torahID == id);
          if(torhaList.length) {
            torhaList[0].status = 1;
          }
          this.getAnnotations();

          cb();

        } else {

          this.api.DeleteTorah(id, success => {
            
            cb();
  
          }, error => {
            console.log('error', error);
            this.util.openDeleteSource(error.messages)
          })
        }
      }
    }
  }

  canDeactivate() {

    var newJson = JSON.stringify(this.safer);
  
    if(newJson != this.saveJson) {
      //console.log('old', this.saveMaamarJson);
      //console.log('new', newJson);
      return false;
    }

    return true;
  }

  saveAndGoBack(successCallback) {

    //console.log('save and go back', this.Maamar);
    this.util.loadingStart();
    this.api.UpdateSefur(this.safer, success => {
      
      this.util.loadingStop();
      this.util.showToast();
      successCallback(true);

     }, error => {
      console.log(error)
      this.util.loadingStop();
      this.util.openDeleteSource(error.messages)
      successCallback(false);
     })
  }
}

enum Tabs {
  links,
  generalInfo,
  torahDetails,
}

export interface ParentComponentApi {
  callBeforeDelete: (id, cb) => void
  callDelete: (id, cb) => void
}