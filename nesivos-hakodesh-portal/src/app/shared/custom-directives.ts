import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[mssautofocus]'
})
export class AutofocusDirective {

  constructor(private host: ElementRef) {}

  ngAfterViewInit() {

    window.setTimeout(() =>
    {
        this.host.nativeElement.focus(); 
    });
  }
}