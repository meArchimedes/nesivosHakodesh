import { NgZone } from '@angular/core';
import { IPDFViewerApplicationOptions } from './options/pdf-viewer-application-options';

export class PinchOnMobileSupport {
  private viewer: any;
  private startX = 0;
  private startY = 0;
  private initialPinchDistance = 0;
  private pinchScale = 1;

  private boundOnViewerTouchStart: any;
  private boundOnViewerTouchMove: any;
  private boundOnViewerTouchEnd: any;

  constructor(private _zone: NgZone) {
    this.boundOnViewerTouchStart = this.onViewerTouchStart.bind(this);
    this.boundOnViewerTouchMove = this.onViewerTouchMove.bind(this);
    this.boundOnViewerTouchEnd = this.onViewerTouchEnd.bind(this);

    this.initializePinchZoom();
  }

  private isMobile() {
    return 'ontouchstart' in document.documentElement;
  }

  private onViewerTouchStart(event: TouchEvent): void {
    this.initialPinchDistance = 0;

    if (event.touches.length === 2) {
      const container = document.getElementById('viewerContainer') as HTMLDivElement;
      const rect = container.getBoundingClientRect();
      if (event.touches[0].pageX >= rect.left && event.touches[0].pageX <= rect.right) {
        if (event.touches[0].pageY >= (rect.top + window.scrollY) && event.touches[0].pageY <= (rect.bottom + window.scrollY)) {
          if (event.touches[1].pageX >= rect.left && event.touches[1].pageX <= rect.right) {
            if (event.touches[1].pageY >= (rect.top + window.scrollY) && event.touches[1].pageY <= (rect.bottom + window.scrollY)) {
              this.startX = (event.touches[0].pageX + event.touches[1].pageX) / 2;
              this.startY = (event.touches[0].pageY + event.touches[1].pageY) / 2;
              this.initialPinchDistance = Math.hypot(event.touches[1].pageX - event.touches[0].pageX, event.touches[1].pageY - event.touches[0].pageY);
              
              if (event.cancelable) {
                event.preventDefault();
              }
              event.stopPropagation();
            }
          }
        }
      }
    }
  }

  private onViewerTouchMove(event: TouchEvent): void {
    const PDFViewerApplicationOptions: IPDFViewerApplicationOptions = (window as any).PDFViewerApplicationOptions;
    const PDFViewerApplication: any = (window as any).PDFViewerApplication;

    if (this.initialPinchDistance <= 0 || event.touches.length !== 2) {
      return;
    }

    const pinchDistance = Math.hypot(event.touches[1].pageX - event.touches[0].pageX, event.touches[1].pageY - event.touches[0].pageY);
    const container = document.getElementById('viewerContainer') as HTMLDivElement;
    const originX = this.startX + container.scrollLeft;
    const originY = this.startY + container.scrollTop;
    this.pinchScale = pinchDistance / this.initialPinchDistance;
    let minZoom = Number(PDFViewerApplicationOptions.get('minZoom'));
    if (!minZoom) {
      minZoom = 0.1;
    }

    const currentZoom = PDFViewerApplication.pdfViewer._currentScale;
    if (currentZoom * this.pinchScale < minZoom) {
      this.pinchScale = minZoom / currentZoom;
    }
    let maxZoom = Number(PDFViewerApplicationOptions.get('maxZoom'));
    if (!maxZoom) {
      maxZoom = 10;
    }
    if (currentZoom * this.pinchScale > maxZoom) {
      this.pinchScale = maxZoom / currentZoom;
    }
    this.viewer.style.transform = `scale(${this.pinchScale})`;
    this.viewer.style.transformOrigin = `${originX}px ${originY}px`;
    
    if (event.cancelable) {
      event.preventDefault();
    }
    event.stopPropagation();
  }

  private onViewerTouchEnd(event: TouchEvent): void {
    const PDFViewerApplication: any = (window as any).PDFViewerApplication;
    if (this.initialPinchDistance <= 0) {
      return;
    }
    this.viewer.style.transform = `none`;
    this.viewer.style.transformOrigin = `unset`;
    PDFViewerApplication.pdfViewer.currentScale *= this.pinchScale;
    const container = document.getElementById('viewerContainer') as HTMLDivElement;
    const rect = container.getBoundingClientRect();
    const dx = this.startX - rect.left;
    const dy = this.startY - rect.top;
    container.scrollLeft += dx * (this.pinchScale - 1);
    container.scrollTop += dy * (this.pinchScale - 1);
    this.resetPinchZoomParams();

    if (event.cancelable) {
      event.preventDefault();
    }
    event.stopPropagation();
  }

  private resetPinchZoomParams(): void {
    this.startX = this.startY = this.initialPinchDistance = 0;
    this.pinchScale = 1;
  }

  public initializePinchZoom(): void {
    if (!this.isMobile()) {
      return;
    }
    this.viewer = document.getElementById('viewer');
    this._zone.runOutsideAngular(() => {
      document.addEventListener('touchstart', this.boundOnViewerTouchStart);
      document.addEventListener('touchmove', this.boundOnViewerTouchMove, { passive: false });
      document.addEventListener('touchend', this.boundOnViewerTouchEnd);
    });
  }

  public destroyPinchZoom(): void {
    if (!this.isMobile()) {
      return;
    }
    document.removeEventListener('touchstart', this.boundOnViewerTouchStart);
    document.removeEventListener('touchmove', this.boundOnViewerTouchMove);
    document.removeEventListener('touchend', this.boundOnViewerTouchEnd);
  }
}
