import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { UtilService } from 'src/app/services/util.service';
import { PdfViewAnnotation, PdfViewRectangle, ShapeType } from '../pdf-view/pdf-view-annotation';

declare const WebViewer: any;

@Component({
  selector: 'app-pdf-view-temp',
  templateUrl: './pdf-view-temp.component.html',
  styleUrls: ['./pdf-view-temp.component.css']
})
export class PdfViewTempComponent implements OnInit {

  protected _pdfSrc;
  protected _annotations: Array<PdfViewAnnotation> = [];
  
  @Input()
  get pdfSrc() {
    return this._pdfSrc;
  }
  set pdfSrc(v) {
    this._pdfSrc = v;
    
    if(this._pdfSrc) {
      this.load();
    }
  }

  @Input()
  set page(pn) {
    //console.log('set page number', pn);

    if(pn) {
      this.wvInstance.docViewer.setCurrentPage(pn);
    }
  }

  @Input()
  get annotations() {
    return this._annotations;
  }
  set annotations(v: Array<PdfViewAnnotation>) {
    this._annotations = v;
  }


  @Input() selectedAnnotationId = null;
  @Output() annotationSelected = new EventEmitter<any>();
  @Output() annotationChanged = new EventEmitter<any>();

  @ViewChild('viewer', {static: true}) viewer: ElementRef;
  wvInstance: any;

  constructor(private util: UtilService) { }

  ngOnInit(): void {
    //console.log('selectedAnnotation', this.selectedAnnotationId);

    this.sendAnnotationSelected = this.sendAnnotationSelected.bind(this);
  }

  

  load(): void {

    this.util.loadingStart();
    WebViewer({
      path: this.util.getAssestPrefix() + 'wv-resources/lib',
    }, this.viewer.nativeElement).then(instance => {

      instance.loadDocument(this.pdfSrc, { filename: 'myfile.pdf' });

      this.wvInstance = instance;
      this.viewer.nativeElement.addEventListener('pageChanged', (e) => {
        //console.log('pageChanged', e);
      });

      instance.docViewer.on('annotationsLoaded', () => {
        //console.log('annotations loaded');

        //


      });
      instance.docViewer.on('documentLoaded', () => {
        //console.log('document loaded');

        //
        if(this.selectedAnnotationId) {

          var find = this.annotations.filter(x => x.id == this.selectedAnnotationId);
          if(find.length > 0) {
            instance.docViewer.setCurrentPage(find[0].pageNumber);
          }
        }

        this.util.loadingStop();

      });
      instance.docViewer.on('pageComplete', (pageNum, canvas) => {
        //console.log('pageComplete ' + pageNum, canvas);
        this.drawAllOnPage(pageNum);
      });

      instance.annotManager.on('annotationChanged', (annotations, action , other) => {
        //console.log('annotationChanged ' + action, annotations);

        this.sendAnnotationChanged(action, annotations[0]);

      });
      instance.annotManager.on('annotationSelected', (annotations, action) => {
        
        //console.log('annotationSelected ' + action, annotations);
        if(action == 'selected') {
          this.sendAnnotationSelected(annotations[0].CustomData);
        } else {
          this.sendAnnotationSelected(null);
        }


      });


      

    })
  }

  sendAnnotationSelected(ann) {
    this.annotationSelected.emit(ann);
  }

  sendAnnotationChanged(action, ann) {

    //
    
    var customData = ann.CustomData;
    
    //console.log('sendAnnotationChanged', customData);

    var mssAction = "modify";

    customData.x = ann.X;
    customData.y = ann.Y;
    customData.w = ann.Width;
    customData.h = ann.Height;

    if(!customData.hasOwnProperty('id')) {

      var newId = Math.floor(Math.random() * 9999) * -1;
      mssAction = 'add';
      customData.id = newId;
      customData.pageNumber = ann.PageNumber;
      customData.customData = {
        torahID: newId,
        status: 0
      };
      //TODO
      customData.shapeType = ShapeType.Rectangle;

    } 

    if(action == 'delete') {
      mssAction = action;
      //////customData.customData.status = 1;
    }

    this.annotationChanged.emit({
      action: mssAction,
      annotation: customData
    });
  }

  drawAllOnPage(pageNumber) {

    var annotManager = this.wvInstance.annotManager;


    //
    var boxesForPage = this.annotations.filter(x => x.pageNumber == pageNumber);
    //console.log(this.annotations);

    boxesForPage.forEach(box => {

      var annot = this.parseAnnotation(box);
      annotManager.addAnnotation(annot);
      annotManager.redrawAnnotation(annot);

      //select if needed
      if(box.id == this.selectedAnnotationId) {
        annotManager.selectAnnotation(annot);
      }
    });
  }

  parseAnnotation(box: PdfViewAnnotation) {

    if(box instanceof PdfViewRectangle) {

      var rectangle = box as PdfViewRectangle;
      const rectangleAnnot = new this.wvInstance.Annotations.RectangleAnnotation();
      rectangleAnnot.PageNumber = rectangle.pageNumber;
      rectangleAnnot.X = rectangle.x;
      rectangleAnnot.Y = rectangle.y;
      rectangleAnnot.Width = rectangle.w;
      rectangleAnnot.Height = rectangle.h;
      rectangleAnnot.Id = rectangle.id;
      rectangleAnnot.CustomData = rectangle;
      return rectangleAnnot;
    }
  }

}
