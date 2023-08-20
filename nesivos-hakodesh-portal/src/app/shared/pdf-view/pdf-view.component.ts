import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { setTime } from 'ngx-bootstrap/chronos/utils/date-setters';

import {PdfViewAnnotation, ShapeType, PdfViewRectangle} from './pdf-view-annotation';

@Component({
  selector: 'app-pdf-view',
  templateUrl: './pdf-view.component.html',
  styleUrls: ['./pdf-view.component.css']
})
export class PdfViewComponent implements OnInit {

  protected _pdfSrc;
  zoom = .7;
  
  @Input()
  get pdfSrc() {
    return this._pdfSrc;
  }
  set pdfSrc(v) {
    this._pdfSrc = v;
  }
  
  @Input() annotations: Array<PdfViewAnnotation> = [];
  @Output() annotationSelected = new EventEmitter<any>();

  currentShapeType: ShapeType = ShapeType.Rectangle;
  createdShape: PdfViewAnnotation; 
  selectedShape: PdfViewAnnotation = null; 
  
  constructor() { }

  ngOnInit(): void {
    console.log('ngOnInit');
  }

  



  pageRendered(e) { 
    
    console.log('pageRendered', e); 
    /*
    var canvasWrapper = e.source.canvas.parentElement;
    canvasWrapper.style.position = 'relative';

    var newCanvas = document.createElement('canvas');
    newCanvas.style.width = canvasWrapper.style.width;
    newCanvas.style.height = canvasWrapper.style.height;
    newCanvas.style.position = 'absolute';
    newCanvas.style.top = '0';
    newCanvas.style.right = '0';
    newCanvas.classList.add("msscanvas");
    newCanvas.setAttribute('mss-page', e.pageNumber);
    canvasWrapper.appendChild(newCanvas);

    console.log(canvasWrapper);
*/
    //setup event listeners
    //e.source.canvas.addEventListener('mousedown', this.startDrawing.bind(this));
    //e.source.canvas.addEventListener('mousemove', this.keepDrawing.bind(this));
    //e.source.canvas.addEventListener('mouseup', this.stopDrawing.bind(this));
    //e.source.canvas.addEventListener('mouseleave', this.stopDrawing.bind(this));

    //check if we already have annotations for this page
    //this.drawAllOnPage(e.source.canvas, e.pageNumber);
  }

  drawAllOnPage(canvas, page) {

    var annotations = this.annotations.filter(x => x.pageNumber == page);
    console.log('ann', annotations);
    annotations.forEach(ann => {
      ann.Draw(canvas);
    });

  }

  /*startDrawing(e) {
    console.log('startDrawing', e);

    //if we have any selected shape from before 
    
    if(this.selectedShape != null) {
      console.log('from before', this.selectedShape);

      var isHover = this.selectedShape.IsHover(e);
      if(!isHover) {
        //if not hover anymore
        
        //remove selections - by clearing and redrawing
        this.selectedShape = null;
        var canvas = e.srcElement;
        //var page = parseInt(canvas.getAttribute('mss-page'));
        var att = canvas.getAttribute('aria-label');
        var page = parseInt(att.replace('Page ', ''));
        var cx = canvas.getContext('2d');


        cx.clearRect(0, 0, canvas.width, canvas.height);
        
        this.drawAllOnPage(canvas, page);
      }
    }

    this.createdShape = new PdfViewRectangle(e.offsetX, e.offsetY);
  }*/

  keepDrawing(e) {

    if (this.createdShape) {

     //move box

    } else {

      
    }
  }
  /*stopDrawing(e) {

    if (this.createdShape) {

      var canvas = e.srcElement;
      //var page = parseInt(canvas.getAttribute('mss-page'));
      var att = canvas.getAttribute('aria-label');
      var page = parseInt(att.replace('Page ', ''));

      var rectangle = this.createdShape as PdfViewRectangle;
      rectangle.w = e.offsetX - rectangle.x;
      rectangle.h = e.offsetY - rectangle.y;
      rectangle.pageNumber = page;

      if(Math.abs(rectangle.w) > 4 && Math.abs(rectangle.h) > 4) {
        
        rectangle.Draw(canvas);
        this.annotations.push(rectangle);
        console.log('stopDrawing ' + page, rectangle);
      }

      this.createdShape = null;
    }

    this.selectOrUnSelect(e);
  }*/

  /*selectOrUnSelect(e) {

    console.log('selectOrUnSelect');

    //check if clickid over an existing shape
    for (let index = 0; index < this.annotations.length; index++) 
    {
      const annotation = this.annotations[index];

      var isHover = annotation.IsHover(e);
      if(isHover) {

        var canvas = e.srcElement;
        console.log('find shape '+ isHover, annotation);
        annotation.Select(canvas);
        this.selectedShape = annotation;
        break;
      }
    }
    
    if(this.selectedShape) {
      this.annotationSelected.emit(this.selectedShape.customData);
    } else {
      this.annotationSelected.emit(null);
    }
  }*/

  afterLoad(e) { /*console.log('afterLoad', e);*/ }
  pageInitialized(e) { /*console.log('pageInitialized', e);*/ }
  textLayerRendered(e) { /*console.log('textLayerRendered', e);*/ }
  onError(e) { /*console.log('onError', e);*/ }
  onProgress(e) { /*console.log('onProgress', e);*/ }

}
