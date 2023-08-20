import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.css']
})
export class AddUserComponent implements OnInit {

  user: any= {};
  onClose: Subject<any>;
  NewUser: any = {}

  sections: Array<any> = [
    {name: 'MAAMARIM_0', display: 'מאמרים - פתגמין קדישין', isMaamarim: true},
    {name: 'MAAMARIM_1', display: 'מאמרים - הדרכות ישרות', isMaamarim: true},
    {name: 'MAAMARIM_2', display: 'מאמרים - בחצר הקודש', isMaamarim: true},
    {name: 'MAAMARIM_3', display: 'מאמרים - הדרכות פרטיות', isMaamarim: true},
    {name: 'TORHAS', display: 'תורת'},
    {name: 'TOPICS', display: 'ענינים'},
    {name: 'SOURCES', display: 'מקורות'},
    {name: 'SOURCES_PRS', display: 'מקורות פרטית'},
  ];

  constructor(public modalRef: BsModalRef, private util: UtilService, private api: ApiService,) { }

  ngOnInit(): void {
    this.onClose = new Subject();
    
    this.util.loadingStart();
    this.api.Getuser(this.user.delailsCopy.id, success => {

      this.util.loadingStop();
      //console.log('Getuser', success);
      this.NewUser = success;

    }, error => {
      this.util.loadingStop();
      console.log(error);
    })
  }

  SaveUser(){

  //  console.log('update user', this.NewUser)

    
    if (this.NewUser.id) {
      //console.log('update')
      this.api.Updateuser(this.NewUser, success => {
        this.modalRef.hide()
        this.onClose.next({
          data: success
        });
      }, error => {
        this.util.openDeleteSource(error.messages)
      })
    }
    else {
      this.api.Adduser(this.NewUser, success => {
        this.modalRef.hide()
        this.onClose.next({
          data: success
        });
       console.log('success add user', success)
      }, error => {
        this.util.openDeleteSource(error.messages)
      })
    }
 
  }

  hasRole(roleName, type) {
    return this.NewUser?.userRoles?.filter(x => x.role.name == `${roleName}_${type}`).length > 0;
  }
  setRole(roleName, type) {

    var fullRoleName = `${roleName}_${type}`;
    //console.log('set role ' + fullRoleName);

    if(this.hasRole(roleName, type)) {
      //remove
      this.NewUser.userRoles = this.NewUser.userRoles.filter(x => x.role.name != fullRoleName);

      //if removing type = view, then remove all other types
      if(type == 'VIEW') {
        this.removeAllSubTypes(roleName);
      }

    } else {

      this.NewUser.userRoles.push({
        role: {
          name: fullRoleName
        }
      });

      //if ading any sub type, make sure also have view type
      if(type != 'VIEW') {

        if(!this.hasRole(roleName, 'VIEW')) {
          this.NewUser.userRoles.push({
            role: {
              name: `${roleName}_VIEW`
            }
          });
        }
      }
    }
  }

  removeAllSubTypes(roleName) {
    this.removeSubType(roleName, 'EDIT');
    this.removeSubType(roleName, 'PRINT');
    this.removeSubType(roleName, 'DOWNLOAD');
  }
  removeSubType(section, type) {

    if(this.hasRole(section, type)) {
      this.NewUser.userRoles = this.NewUser.userRoles.filter(x => x.role.name != `${section}_${type}`);
    }
  }
}
