import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

declare const WebViewer: any;

@Component({
  selector: 'app-link-maamar',
  templateUrl: './link-maamar.component.html',
  styleUrls: ['./link-maamar.component.css']
})
export class LinkMaamarComponent implements OnInit {

  Maamar: any = {};
  SelectedSafer: any = {};
  onClose: Subject<any>;
  ShowSafer: boolean = false;
  selectedAnnatation: any = {};

  constructor(public modalRef: BsModalRef, private api: ApiService, private util: UtilService) { }

  ngOnInit(): void {
    this.onClose = new Subject();
  }

  selectionUpdated(e) {
    this.SelectedSafer = e;
    this.ShowSafer = true;
  }

  TorahSelected(e) {
   // console.log('TorahSelected', e);
    this.selectedAnnatation = e;
  }

  connect() {
   console.log('connect', this.selectedAnnatation);

    if(this.selectedAnnatation.hasOwnProperty('torahID')) {

      this.selectedAnnatation.sefer = this.SelectedSafer;

      var newLinks  = [];
      newLinks.push({
        maamar: this.Maamar,
        torah: this.selectedAnnatation
      });
      this.util.loadingStart();
      this.api.AddMaamarTorahLink(newLinks, success => {
        this.util.loadingStop();
        this.onClose.next({
          success
        });
        this.modalRef.hide()
      }, error => {
        this.util.loadingStop();
        console.log('error', error)
        this.util.openDeleteSource(error.messages)
      });

    } else {
      this.util.openDeleteSource("אנא בחר תיבה");
    }
  }
}
