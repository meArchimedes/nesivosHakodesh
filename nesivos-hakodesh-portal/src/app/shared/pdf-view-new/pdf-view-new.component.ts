import { Component, EventEmitter, HostListener, Input, OnInit, Output } from '@angular/core';
import { ParentComponentApi } from 'src/app/components/main/sefurim/safer-pdf/safer-pdf.component';
import { UtilService } from 'src/app/services/util.service';
import { PdfViewAnnotation, PdfViewRectangle } from '../pdf-view/pdf-view-annotation';
import { pdfDefaultOptions } from './../lib/options/pdf-default-options';


@Component({
  selector: 'app-pdf-view-new',
  templateUrl: './pdf-view-new.component.html',
  styleUrls: ['./pdf-view-new.component.css']
})
export class PdfViewNewComponent implements OnInit {

  protected _pdfSrc;
  @Input()
  set pdfSrc(v) {
    this._pdfSrc = v;
  }
  get pdfSrc() {
    return this._pdfSrc;
  }

  protected _page: number = 1;
  @Input()
  set page(pn) {
    if(pn) {
      this._page = pn;
    }
  }
  get page() {
    return this._page;
  }

  protected _annotations: Array<PdfViewAnnotation> = [];
  @Input() 
  set annotations(v: Array<PdfViewAnnotation>) {
    this._annotations = v;
    this.reload(this.currentPage);
  }
  get annotations() {
    return this._annotations;
  }


  @Input() parentApi: ParentComponentApi;
  @Input() selectedAnnotationId: number = 0;
  @Output() annotationSelected = new EventEmitter<any>();
  @Output() annotationChanged = new EventEmitter<any>();

  reloadPage = 0;
  currentPage;
  activeAnnotation: PdfViewAnnotation;
  activeAnnotationBackup: PdfViewAnnotation;
  isMoving: boolean = false;
  isNew: boolean = false;
  scale: number = 0;
  viewType: string = 'hand';
  DEFAULT_CURSET_TYPE = 'crosshair';
  curserType: string = this.DEFAULT_CURSET_TYPE;

  constructor(private util: UtilService) { 

    pdfDefaultOptions.assetsFolder = this.util.getAssestPrefix() + 'assets';
  }

  ngOnInit(): void {
  }

  ViewHand() {
    this.viewType = 'hand';
  }
  ViewText() {
    this.viewType = 'text';
  }
  ViewAnn() {
    this.viewType = 'ann';
  }
  IsAnnToll() {
    return this.viewType == 'ann';
  }

  @HostListener('window:keyup', ['$event']) 
  keyEvent(event: KeyboardEvent) {
    //console.log('HostListener', event);

    if(event.key == "Delete" && this.IsAnnToll()) {

      var selectedAnnotation = this.getSelectedAnnotation();
      if(selectedAnnotation) {
        //console.log('delete selectedAnnotation', selectedAnnotation);

        if(this.parentApi) {
          this.parentApi.callBeforeDelete(selectedAnnotation.id, () => {

            //console.log('callback true');
            this.parentApi.callDelete(selectedAnnotation.id, () => {

              //console.log('deleted, need to remove it from list, and reload page');
              selectedAnnotation.customData.status = 1;
              this.annotations = this.annotations.filter(x => x.id != selectedAnnotation.id);

              this.annotationChanged.emit({
                action: 'delete',
                annotation: selectedAnnotation
              });

              this.selectedAnnotationId = 0;
              this.sendUpdateAfterSelect(selectedAnnotation.id, selectedAnnotation.pageNumber);

            });
          });
        }
      }
    }
  }

  onPagesLoaded(e) { 
    //console.log('onPagesLoaded', e); 

    var selectedAnnotation = this.getSelectedAnnotation();
    if(selectedAnnotation) {

      //console.log('selectedAnnotation', selectedAnnotation);
      this._page = selectedAnnotation.pageNumber;

    }
  }

  onPageRendered(pageEvent) { 
    //console.log('onPageRendered', pageEvent.pageNumber);
    this.scale = pageEvent.source.scale;

    var canvas = pageEvent.source.canvas;
    canvas.addEventListener("mousedown", this.onMouseDown.bind(this));
    canvas.addEventListener("mousemove", this.onMouseMove.bind(this));
    canvas.addEventListener("mouseup", this.onMouseUp.bind(this));
  
    this.drawAllOnPage(pageEvent);
  }

  drawAllOnPage(pageEvent) {

    var annotations = this.annotations.filter(x => x.pageNumber == pageEvent.pageNumber);
    //console.log('ann', annotations);
    annotations.forEach(ann => {
      ann.Draw(pageEvent.source);

      if(this.selectedAnnotationId == ann.id) {
        ann.Select(pageEvent.source.canvas);
      }

    });
  }

  setActiveAnnotation(newActiveAnnotation) {
    this.activeAnnotation = newActiveAnnotation;
    this.activeAnnotationBackup = JSON.parse(JSON.stringify(newActiveAnnotation));
  }

  onMouseDown(e) {

    if(this.IsAnnToll()) {

      var pageNumber = this.getPageNumber(e.srcElement);
      //console.log('onMouseDown', pageNumber);
  
      var savePrev = this.selectedAnnotationId;
  
      //first check if moving current selected,
      var selectedAnnotation = this.getSelectedAnnotation();
      if(selectedAnnotation != null && selectedAnnotation.pageNumber == pageNumber && selectedAnnotation.IsOnMovingSpot(e)) {
  
        this.setActiveAnnotation(selectedAnnotation);
        this.sendUpdateAfterSelect(savePrev, pageNumber);
        this.isMoving = true;
        return;
      }
  
      var annotationsForPage = this.annotations.filter(x => x.pageNumber == pageNumber);
      
      //if not find, if moving any other
      this.selectedAnnotationId = 0;
      for (let index = 0; index < annotationsForPage.length; index++) 
      {
        const annotation = annotationsForPage[index];
  
        if(annotation.IsOnMovingSpot(e)) {
          this.selectedAnnotationId = annotation.id;
          this.setActiveAnnotation(annotation);
          break;
        }
      }
  
      //if not start a new 
      if(this.selectedAnnotationId == 0) {
  
        //console.log('start new real x', e.offsetX);
        //console.log('start new real y', e.offsetY);
        //console.log('start new  x', e.offsetX / this.scale);
        //console.log('start new  y', e.offsetY / this.scale);
  
        //TODO based on type
        var rect = new PdfViewRectangle();
        rect.x = e.offsetX / this.scale;
        rect.y = e.offsetY / this.scale;
        rect.h = 0;
        rect.w = 0;
        rect.scale = this.scale;
        rect.pageNumber = pageNumber;
        rect.customData = {
          isNew: true,
          newId: rect.id,
          status: 0
        };
        //console.log('start new', rect);
        rect.IsOnMovingSpot(e);
  
        this.setActiveAnnotation(rect);
        this.annotations.push(this.activeAnnotation);
        this.isNew = true;
      }
  
      this.sendUpdateAfterSelect(savePrev, pageNumber);
      this.isMoving = true;
  
      //console.log('activeAnnotation', this.activeAnnotation);
    }
  }

  onMouseMove(e) {

    //console.log('ismoving', this.isMoving);
    if(this.IsAnnToll()) {

      if(this.isMoving) {
        this.activeAnnotation.Move(e);
        this.reload(this.activeAnnotation.pageNumber);

        if(this.curserType == this.DEFAULT_CURSET_TYPE && !this.isNew) {
          this.setCurserType(e);
        }

      } else {

        this.setCurserType(e);
      }
    }
  }

  onMouseUp(e) {

    this.isMoving = false;
    
    if(this.IsAnnToll()) {

      //fix negetive w or h
      this.activeAnnotation.FixBeforeSave();
  
      //if new annotation
      if(this.isNew) {
  
        //if bit anouge, send notification
        if(this.activeAnnotation.isBigEnough()) {
  
          this.annotationChanged.emit({
            action: 'add',
            annotation: this.activeAnnotation
          });
        } else {
  
          //remove from list
          this.annotations = this.annotations.filter(x => x.id != this.activeAnnotation.id);
          this.reload(this.activeAnnotation.pageNumber);
        }
      } else {
  
        //check if it was changed
        if(this.activeAnnotation.IsChanged(this.activeAnnotationBackup)) {
  
          this.annotationChanged.emit({
            action: 'modify',
            annotation: this.activeAnnotation
          });
        }
      }
    }

    this.isNew = false;
  }

  setCurserType(e) {

    this.curserType = '';

    //change curser type
    var pageNumber = this.getPageNumber(e.srcElement);
    var annotationsForPage = this.annotations.filter(x => x.pageNumber == pageNumber);

    //first check if on top of selected,
    var selectedAnnotation = this.getSelectedAnnotation();
    if(selectedAnnotation != null) {

      var curserType = selectedAnnotation.GetMouseMovingType(e);
      if(curserType) {
        this.curserType = curserType;
      }
    
    } else {

      for (let index = 0; index < annotationsForPage.length; index++) 
      {
        const annotation = annotationsForPage[index];
        
        var curserType = annotation.GetMouseMovingType(e);
        if(curserType) {
          this.curserType = 'pointer';
          break;
        }
      }
    }

    if(this.curserType == '') {
      this.curserType = this.DEFAULT_CURSET_TYPE;
    }

  }

  getSelectedAnnotation() {

    if(this.selectedAnnotationId != 0) {

      var find = this.annotations.filter(x => x.id == this.selectedAnnotationId);
      //console.log('find for ' + this.selectedAnnotationId, find);
      if(find.length > 0) {
        return find[0];
      }
    }

    return null;
  }

  sendUpdateAfterSelect(savePrev, pageNumber) {
    //console.log('Prev ' + savePrev, this.selectedAnnotationId);
    //if change
    if(this.selectedAnnotationId != savePrev) {

      var selectedAnnotation = this.getSelectedAnnotation();
      if(selectedAnnotation) {
        this.annotationSelected.emit(selectedAnnotation);
      } else {
        this.annotationSelected.emit(null);
      }
  
      this.reload(pageNumber);
    }
  }

  getPageNumber(canvas) {
    var att = canvas.getAttribute('aria-label');
    var page = parseInt(att.replace('Page ', ''));
    return page;
  }

  reload(page) {

    this.reloadPage = page;

    setTimeout(() => {
      this.reloadPage = 0;
    });
  }





  
  updateCurrentZoomFactor(e) { /*console.log('updateCurrentZoomFactor', e);*/ }
  onTextLayerRendered(e) { /*console.log('onTextLayerRendered', e);*/ }
  onPdfLoaded(e) { /*console.log('onPdfLoaded', e);*/ }
  onZoomChange(e) { /*console.log('onZoomChange', e);*/ }
  pageChange(e) { /*console.log('pageChange', e);*/ }
}
