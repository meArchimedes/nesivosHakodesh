import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-user-popup',
  templateUrl: './user-popup.component.html',
  styleUrls: ['./user-popup.component.css']
})
export class UserPopupComponent implements OnInit {

  NewTopice: any = {
    category: {
      categoryName: '',
    },
    
    subTopices : []
  }
 
  Topices: any= {
    Categories : []
  };
  onClose: Subject<any>;
  constructor(public modalRef: BsModalRef, private util: UtilService, private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    
    //console.log('set  Topice', this.Topices)
    if (this.Topices.delailsCopy != false) {
      
      this.NewTopice = this.Topices.delailsCopy
      if (this.NewTopice.subTopices == null) {
        this.NewTopice.subTopices = []
      }
       
     }
     if (this.NewTopice.categoryName != 'ספרים') {
      this.addSubTopices();
     }
    
  }
  addSubTopices(){
   // console.log('new topice',  this.NewTopice)
    this.NewTopice.subTopices.push({})
  }

  ChangeCategory(category){
   // console.log('event', category)
    this.NewTopice.category.categoryName = category;
  //  console.log(' this.NewTopice.category ',  this.NewTopice.category )
  }

  Save(){
   // console.log('save topice', this.NewTopice)
    

    if (this.NewTopice.topicID) {
     
       this.api.UpdateTopice(this.NewTopice, success => {
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
      this.api.AddTopice(this.NewTopice, success => {
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

  // =================     testing    ===========
  Test(tab3){
 //console.log('tab3', tab3)
  }

}
