import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-torahs-new-popup',
  templateUrl: './torahs-new-popup.component.html',
  styleUrls: ['./torahs-new-popup.component.css']
})
export class TorahsNewPopupComponent implements OnInit {

  onClose: Subject<any>;
  contact: any= {};
  NewTorah: any = {};
  obListOfParsha: any = [];
  constructor(  
   public modalRef: BsModalRef,
    private util: UtilService,
    private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    this.loadParshas();
  }

  Save(details){
    this.api.AddTorah(this.NewTorah, success => {
  if (details) {
    
    this.util.navigate('torahs/' + success.torahID)
    this.modalRef.hide()
  }
    this.onClose.next({
      data: success
    });
    this.modalRef.hide()
    }, error => {
      console.log(error)
      this.util.openDeleteSource(error.messages)
     // this.openDeleteMaamar(error.messages)

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


}
