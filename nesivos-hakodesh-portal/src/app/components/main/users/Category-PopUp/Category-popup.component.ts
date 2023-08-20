import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-Category-popup',
  templateUrl: './Category-popup.component.html',
  styleUrls: ['./Category-popup.component.css']
})
export class CategoryPopupComponent implements OnInit {

  NewCategory: any = {}
 
  
  onClose: Subject<any>;
  constructor(public modalRef: BsModalRef, private util: UtilService, private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
  //  console.log('save topice', this.NewCategory)
    if (this.NewCategory.delailsCopy != false) {
      
      this.NewCategory = this.NewCategory.delailsCopy
     
      
     }
    }
  Save(){
 //   console.log('save topice', this.NewCategory)
    

    if (this.NewCategory.id) {
     
       this.api.UpdateCategory(this.NewCategory, success => {
        this.modalRef.hide()
        this.onClose.next({
          data: success
        });
       }, error => {
        console.log(error)
        this.util.openDeleteSource(error.messages)
       })
    }
    else{
      this.api.AddCategory(this.NewCategory, success => {
       // console.log(success)
      
      this.modalRef.hide()
        this.onClose.next({
          data: success
        });
      
        }, error => {
          console.log(error)
          this.util.openDeleteSource(error.messages)
    
        })
    }

  }

}
